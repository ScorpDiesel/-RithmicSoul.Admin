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
    [Parameter] public bool ReturnIdOnInsert { get; set; }
    [Parameter] public Type TEditDialog { get; set; }
    [Parameter] public Type TDialog { get; set; }
    [Parameter] public List<int> FilterIds { get; set; }
    [Parameter] public string ColumnToFilter { get; set; }
    [Parameter] public IEnumerable<T> Items { get; set; }

    protected IEnumerable<PropertyInfo> _properties;
    protected MudDataGrid<T> thisDataGrid;
    private Dictionary<(string, int), string> RowHighlight = new();
    private AppSettings _appSettings;
    protected bool _isExpanded;
    protected string TableName;
    protected bool _showContent;
    protected string _contentStyle;
    protected int _propertiesCount;

    protected override async Task OnInitializedAsync()
    {
        Initialize();
        await FilterItemsAsync();
    }

    protected void Initialize()
    {
        _appSettings = AppSettingsOptions.Value;
        _contentStyle = "display: none;";
        TableName = typeof(T).Name.Replace("Dto", string.Empty).SplitCamelCase();
        _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
    }

    private async Task FilterItemsAsync()
    {
        if (FilterIds is null) return;

        var allResults = await ApiService?.GetAllAsync();
        IList<T> filteredResults = new List<T>();
        foreach (var item in allResults)
        {
            var property = item.GetType().GetProperties().FirstOrDefault(p => p.Name == ColumnToFilter);
            var value = property?.GetValue(item, null);

            if (value is null) continue;
            if (FilterIds.Contains((int)value)) filteredResults?.Add(item);
        }

        Items = filteredResults;
    }

    protected void ExpandGroups()
    {
        thisDataGrid?.ExpandAllGroups();
        _isExpanded = true;
    }

    protected void CollapseGroups()
    {
        thisDataGrid?.CollapseAllGroups();
        _isExpanded = false;
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
            // Create a parameter for the lambda expression
            var parameterExp = Expression.Parameter(typeof(T), propertyInfo.Name);

            // Create a property access expression for the specified property
            var propertyExp = Expression.Property(parameterExp, propertyInfo);

            // Because Property expects a Func<T, object>, we may need to convert the property 
            // expression to object if the property type is a value type
            var convertExp = Expression.Convert(propertyExp, typeof(object));

            // Create a lambda expression for the property access expression
            var lambdaExp = Expression.Lambda<Func<T, object>>(convertExp, parameterExp);
            
            builder.OpenComponent(0, typeof(PropertyColumn<T, object>));
            builder.AddAttribute(1, "Property", lambdaExp);
            builder.AddAttribute(2, "Title", propertyInfo.Name.SplitCamelCase());
            if (propertyInfo.Name == GroupBy) builder.AddAttribute(3, _appSettings.RsDataGridRenderFragmentGroupingAttribute, true);
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
            bool isSuccessful = false;
            var data = result.Data;

            if (data.IsGenericList())
            {
                resultData = (List<T>)data;
                isSuccessful = await BulkActionsService.BulkInsertAsync(resultData);
            }
            else
            {
                resultData = (T)result.Data;
                await UpdateItemAsync(resultData);
            }

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