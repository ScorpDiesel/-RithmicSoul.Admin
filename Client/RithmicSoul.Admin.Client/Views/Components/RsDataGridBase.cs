using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services;
using RithmicSoul.Admin.Application.Interfaces.Services.Survey;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Models;
using RithmicSoulSharedLibrary.Extensions;

namespace RithmicSoul.Admin.Client.Views.Components;

public class RsDataGridBase<T> : ComponentBase where T : class
{
    [Inject] private IBulkActionsService<T> BulkActionsService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public IService<T> ApiService { get; set; }
    [Parameter] public IDatabaseService<T> ApiDatabaseService { get; set; }
    [Parameter] public bool CanGroup { get; set; }
    [Parameter] public bool GroupExpanded { get; set; }
    [Parameter] public string GroupBy { get; set; }
    [Parameter] public bool MultiSelection { get; set; }
    [Parameter] public int[] ColumnsToHide { get; set; }
    [Parameter] public bool CanEdit { get; set; }
    [Parameter] public bool CanCreate { get; set; }
    [Parameter] public bool CanDelete { get; set; }
    [Parameter] public bool CanFilter { get; set; }
    [Parameter] public bool ReturnIdOnInsert { get; set; }
    [Parameter] public Type TEditDialog { get; set; }
    [Parameter] public Type TDialog { get; set; }
    //[Parameter] public List<int> NoFilterIds { get; set; }
    [Parameter] public string ColumnToFilter { get; set; }
    [Parameter] public IEnumerable<T> Items { get; set; }
    [Parameter] public TableColumnWidth TableColumnWidths { get; set; }

    protected IEnumerable<PropertyInfo> _properties;
    protected MudDataGrid<T> thisDataGrid;
    private Dictionary<(string, int), string> RowHighlight = new();
    private AppSettings _appSettings;
    protected string TableName;
    protected bool _showContent;
    protected string _contentStyle;
    protected int _propertiesCount;
    protected string _searchString;

    protected override async Task OnInitializedAsync()
    {
        Initialize();
    }

    protected void Initialize()
    {
        _appSettings = AppSettingsOptions.Value;
        _contentStyle = "display: none;";
        TableName = typeof(T).Name.Replace("Dto", string.Empty);//.SplitCamelCase();
        _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
    }

    //private async Task FilterItemsAsync()
    //{
    //    if (FilterIds is null) return;

    //    var allResults = await ApiService?.GetAllAsync();
    //    IList<T> filteredResults = new List<T>();
    //    foreach (var item in allResults)
    //    {
    //        var property = item.GetType().GetProperties().FirstOrDefault(p => p.Name == ColumnToFilter);
    //        var value = property?.GetValue(item, null);

    //        if (value is null) continue;
    //        if (FilterIds.Contains((int)value)) filteredResults?.Add(item);
    //    }

    //    Items = filteredResults;
    //}

    protected Func<T, bool> QuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;
        foreach (var property in _properties)
        {
            var value = property.GetValue(x, null);
            if (value is null) continue;
            if (value.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase)) return true;
        }

        return false;
    };

    protected void ExpandGroups()
    {
        thisDataGrid?.ExpandAllGroups();
        GroupExpanded = true;
    }

    protected void CollapseGroups()
    {
        thisDataGrid?.CollapseAllGroups();
        GroupExpanded = false;
    }

    //protected List<RenderFragment> CreateColumn()
    //{
    //    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
    //    var columns = new List<RenderFragment>();
    //    foreach (var item in properties.Select((value, index) => new { index, value }))
    //    {
    //        var property = item.value;
    //        var thisIndex = item.index + 1;
    //        if (ColumnsToHide is not null && ColumnsToHide.Contains(thisIndex)) continue;

    //        Dictionary<string, object> attributeDictionary = new();
    //        var parameterExp = Expression.Parameter(typeof(T), property.Name);
    //        var propertyExp = Expression.Property(parameterExp, property);
    //        var convertExp = Expression.Convert(propertyExp, typeof(object));
    //        var lambdaExp = Expression.Lambda<Func<T, object>>(convertExp, parameterExp);

    //        attributeDictionary[_appSettings.RsDataGridRenderFragmentPropertyAttribute] = lambdaExp;
    //        attributeDictionary[_appSettings.RsDataGridRenderFragmentTitleAttribute] =
    //            property.Name.SplitCamelCase();
    //        if (property.Name == GroupBy)
    //            attributeDictionary[_appSettings.RsDataGridRenderFragmentGroupingAttribute] = true;

    //        columns.Add(CreateRenderFragment(attributeDictionary, typeof(PropertyColumn<T, object>)));
    //    }
    //    return columns;
    //}

    protected RenderFragment CreateColumn(PropertyInfo propertyInfo)
    {
        ++_propertiesCount;
        return builder =>
        {
            var propertyName = propertyInfo.Name;
            var parameterExp = Expression.Parameter(typeof(T), propertyName);
            var propertyExp = Expression.Property(parameterExp, propertyInfo);
            var convertExp = Expression.Convert(propertyExp, typeof(object));
            var lambdaExp = Expression.Lambda<Func<T, object>>(convertExp, parameterExp);
            builder.OpenComponent(0, typeof(PropertyColumn<T, object>));
            builder.AddAttribute(1, "Property", lambdaExp);
            builder.AddAttribute(2, "Title", propertyInfo.Name.SplitCamelCase());
            if (TableColumnWidths is not null && TableColumnWidths.TableName == TableName 
                                              && TableColumnWidths.ColumnWidths.TryGetValue(_propertiesCount, out var width)) builder.AddAttribute(3, "CellStyle", $"width: { width }");
            if (propertyInfo.Name == GroupBy) builder.AddAttribute(4, _appSettings.RsDataGridRenderFragmentGroupingAttribute, true);
            builder.CloseComponent();
            ShowContent();
        };
    }

    protected async Task CreateNewItemAsync<T>(object data)
    {
        dynamic resultData;
        bool isSuccessful = false;

        if (data.IsGenericList())
        {
            resultData = (List<T>)data;
            isSuccessful = await BulkActionsService.BulkInsertAsync(resultData);
        }
        else
        {
            resultData = (T)data;
            if (ReturnIdOnInsert)
            {
                var id = await ApiService?.InsertForIdAsync(resultData);
                isSuccessful = id is int; //TODO: Figure out what to do with the id
            }
            else
            {
                isSuccessful = await ApiService?.InsertAsync(resultData);
            }
        }

        string message;

        if (isSuccessful)
        {
            message = string.Format(_appSettings.ItemCreatedSuccessMessageTemplate, TableName);
            await GetAllItemsAsync();
        }
        else
        {
            message = string.Format(_appSettings.ItemCreatedFailureMessageTemplate, TableName);
        }

        ShowSnackBar(isSuccessful, message);
    }

    protected RenderFragment CreateRenderFragment(Dictionary<string, object> attributeDictionary, Type componentType)
    {
        return builder =>
        {
            var seq = 0;
            builder.OpenComponent(seq, componentType);
            foreach (var attribute in attributeDictionary)
            {
                var name = attribute.Key;
                var value = attribute.Value;
                builder.AddAttribute(++seq, name, value);
            }

            builder.CloseComponent();
        };
    }

    public async Task ShowNewItemDialogAsync()
    {
        var dialog = await DialogService?.ShowAsync(TDialog, $"New {TableName}")!;
        var result = await dialog.Result;
        if (!result.Canceled) await CreateNewItemAsync<T>(result.Data);
    }

    protected async Task UpdateItemAsync(T dto)
    {
        SetRowHighlight(dto);
        var response = await ApiService?.UpdateAsync(dto)!;
        ResetRowHighlight(dto);
        StateHasChanged();
        ShowSnackBar(response, string.Format(_appSettings.ItemUpdatedSuccessMessageTemplate, TableName));
        await GetAllItemsAsync();
    }

    public async Task DeleteSelectedItemsAsync(T item = null)
    {
        var options = new DialogOptions { CloseButton = true };
        var parameters = new DialogParameters
        {
            { "ContentText", "Delete these/this record(s)?" },
            { "CloseButtonText", "Delete" },
            { "CancelButtonText", "No" },
            { "Style", "min-width:300px" },
            { "Color", Color.Error }
        };
        
        var dialog = await DialogService.ShowAsync<ActionDialog>("Delete", parameters, options)!;
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
            var isSuccess = false;
            if (item is not null)
            {
                isSuccess = await ApiService?.DeleteAsync(item)!;
            }
            else
            {
                var items = thisDataGrid.SelectedItems;
                if (!items.Any())
                {
                    ShowSnackBar(isSuccess, "No items are selected to delete");
                    return;
                }

                isSuccess = await BulkActionsService.BulkDeleteAsync(items.ToList())!;
            }
            
            string message;

            if (isSuccess)
            {
                message = string.Format(_appSettings.ItemDeletedSuccessMessageTemplate, TableName);
                await GetAllItemsAsync();
            }
            else
            {
                message = string.Format(_appSettings.ItemDeletedFailureMessageTemplate, TableName);
            }

            ShowSnackBar(isSuccess, message);
        }
    }

    public async Task ShowEditItemDialogAsync<T>(T dto)
    {
        SetRowHighlight(dto);
        var options = new DialogOptions { CloseButton = true };
        var dialogParameters = new DialogParameters { { "Model", dto } };
        IDialogReference dialog;

        if (TEditDialog is not null)
        {
            dialog = await DialogService?.ShowAsync(TEditDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, TableName), dialogParameters, options)!;
        }
        else
        {
            dialog = await DialogService?.ShowAsync(TDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, TableName), dialogParameters, options)!;
        }

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            dynamic resultData;
            var isSuccess = false;
            var data = result.Data;

            if (data.IsGenericList())
            {
                resultData = (List<T>)data;
                isSuccess = await BulkActionsService.BulkInsertAsync(resultData);
            }
            else
            {
                resultData = (T)result.Data;
                await UpdateItemAsync(resultData);
            }

            string message;

            if (isSuccess)
            {
                message = string.Format(_appSettings.ItemDeletedSuccessMessageTemplate, TableName);
                await GetAllItemsAsync();
            }
            else
            {
                message = string.Format(_appSettings.ItemDeletedFailureMessageTemplate, TableName);
            }

            ShowSnackBar(isSuccess, message);

        }
    }

    protected void SetRowHighlight(object dto)
    {
        RowHighlight[(dto.GetType().Name, dto.GetHashCode())] = _appSettings.RowHighlightBgColor;
    }

    protected string GetRowHighlight(object dto, int rowIndex)
    {
        return RowHighlight.TryGetValue((dto.GetType().Name, dto.GetHashCode()), out var bgColor) ? string.Format(_appSettings.RowHighlightBgColorTemplate, bgColor) : string.Empty;
    }

    protected void ResetRowHighlight(object dto)
    {
        RowHighlight?.Remove((dto.GetType().Name, dto.GetHashCode()));
    }

    protected void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar?.Clear();
        Snackbar?.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    public async Task GetAllItemsAsync()
    {
        var items = await ApiService.GetAllAsync();
        Items = items;
    }

    protected void ShowContent()
    {
        if (_propertiesCount != _properties.Count()) return;
        _contentStyle = "";
        _showContent = true;
        StateHasChanged();
    }
}