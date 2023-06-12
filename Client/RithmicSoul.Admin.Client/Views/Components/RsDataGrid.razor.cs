using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsDataGrid<T> : ComponentBase where T : class
{
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IDialogService? DialogService { get; set; }
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public IService<T>? ApiService { get; set; }
    [Parameter] public bool CanGroup { get; set; }
    [Parameter] public bool GroupExpanded { get; set; }
    [Parameter] public string? GroupBy { get; set; }
    [Parameter] public bool MultiSelection { get; set; }
    [Parameter] public int[]? ColumnsToHide { get; set; }
    [Parameter] public bool CanEdit { get; set; }
    [Parameter] public bool CanCreate { get; set; }
    [Parameter] public bool CanDelete { get; set; }
    [Parameter] public bool ReturnIdOnInsert { get; set; }
    [Parameter] public Type? TEditDialog { get; set; }
    [Parameter] public Type TDialog { get; set; }
    [Parameter] public List<int>? FilterIds { get; set; }
    [Parameter] public string? ColumnToFilter { get; set; }
    [Parameter] public IEnumerable<T> Items { get; set; }

    private IEnumerable<PropertyInfo> _properties;
    private MudDataGrid<T> thisDataGrid;
    private Dictionary<(string, int), string> RowHighlight = new();
    private AppSettings _appSettings;
    private bool _isExpanded;
    protected string TableName;
    private string _contentStyle;
    private string _loadingStyle;

    protected override async Task OnInitializedAsync()
    {
        SetFields();
        await FilterItemsAsync();
    }

    private void SetFields()
    {
        _appSettings = AppSettingsOptions.Value;
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

    private void ExpandGroups()
    {
        thisDataGrid?.ExpandAllGroups();
        _isExpanded = true;
    }

    private void CollapseGroups()
    {
        thisDataGrid?.CollapseAllGroups();
        _isExpanded = false;
    }

    protected List<RenderFragment> CreateColumn()
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        var columns = new List<RenderFragment>();
        foreach (var item in properties.Select((value, index) => new { index, value }))
        {
            var property = item.value;
            var thisIndex = item.index + 1;
            if (ColumnsToHide is not null && ColumnsToHide.Contains(thisIndex)) continue;

            Dictionary<string, object> attributeDictionary = new();
            var parameterExp = Expression.Parameter(typeof(T), property.Name);
            var propertyExp = Expression.Property(parameterExp, property);
            var convertExp = Expression.Convert(propertyExp, typeof(object));
            var lambdaExp = Expression.Lambda<Func<T, object>>(convertExp, parameterExp);

            attributeDictionary[_appSettings.RsDataGridRenderFragmentPropertyAttribute] = lambdaExp;
            attributeDictionary[_appSettings.RsDataGridRenderFragmentTitleAttribute] =
                property.Name.SplitCamelCase();
            if (property.Name == GroupBy)
                attributeDictionary[_appSettings.RsDataGridRenderFragmentGroupingAttribute] = true;

            columns.Add(CreateRenderFragment(attributeDictionary, typeof(PropertyColumn<T, object>)));
        }
        return columns;
    }

    private RenderFragment CreateColumn(PropertyInfo propertyInfo)
    {
        Dictionary<string, object> attributeDictionary = new();
        var parameterExp = Expression.Parameter(typeof(T), propertyInfo.Name);
        var propertyExp = Expression.Property(parameterExp, propertyInfo);
        var convertExp = Expression.Convert(propertyExp, typeof(object));
        var lambdaExp = Expression.Lambda<Func<T, object>>(convertExp, parameterExp);

        attributeDictionary[_appSettings.RsDataGridRenderFragmentPropertyAttribute] = lambdaExp;
        attributeDictionary[_appSettings.RsDataGridRenderFragmentTitleAttribute] = propertyInfo.Name.SplitCamelCase();
        if (propertyInfo.Name == GroupBy)
            attributeDictionary[_appSettings.RsDataGridRenderFragmentGroupingAttribute] = true;

        return CreateRenderFragment(attributeDictionary, typeof(PropertyColumn<T, object>));
    }

    private async Task CreateNewItemAsync<T>(object data)
    {
        dynamic resultData;
        bool isSuccessful;

        if (data.IsGenericList())
        {
            resultData = (List<T>)data;
            isSuccessful = await ApiService?.BulkInsertAsync(resultData);
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
            await UpdateItemsAsync();
        }
        else
        {
            message = string.Format(_appSettings.ItemCreatedFailureMessageTemplate, TableName);
        }

        ShowSnackBar(isSuccessful, message);
    }

    private RenderFragment CreateRenderFragment(Dictionary<string, object> attributeDictionary, Type componentType)
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

    private async Task UpdateItemAsync(T dto)
    {
        SetRowHighlight(dto);
        var response = await ApiService?.UpdateAsync(dto)!;
        ResetRowHighlight(dto);
        StateHasChanged();
        ShowSnackBar(response, string.Format(_appSettings.ItemUpdatedSuccessMessageTemplate, TableName));
    }

    public async Task DeleteAsync<T>(T dto)
    {
        SetRowHighlight(dto);
        var dialog = await DialogService?.ShowAsync<DeleteItemDialog>(null)!;
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            dynamic dtoList = new List<T> { dto }; //TODO: Implement checkbox selection in each row to add to collection to be sent to BulkDelete
            var response = await ApiService?.BulkDeleteAsync(dtoList)!;
            string message;

            if (response)
            {
                message = string.Format(_appSettings.ItemDeletedSuccessMessageTemplate, TableName);
            }
            else
            {
                message = string.Format(_appSettings.ItemDeletedFailureMessageTemplate, TableName);
            }

            await UpdateItemsAsync();
            ShowSnackBar(response, message);
        }
    }

    public async Task ShowEditItemDialogAsync<T>(T dto)
    {
        SetRowHighlight(dto);
        var dialogParameters = new DialogParameters { {"Model", dto} };
        IDialogReference? dialog;

        if (TEditDialog is not null)
        {
            dialog = await DialogService?.ShowAsync(TEditDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, TableName), dialogParameters)!;
        }
        else
        {
            dialog = await DialogService?.ShowAsync(TDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, TableName), dialogParameters)!;
        }

        var result = await dialog.Result;
        ResetRowHighlight(dto);
        StateHasChanged();
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

    void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar?.Clear();
        Snackbar?.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    public async Task UpdateItemsAsync()
    {
        var items = await ApiService?.GetAllAsync();
        Items = items;
    }

    private void DisplayContent(EventArgs obj)
    {
        _contentStyle = "";
        _loadingStyle = "display: none;";
    }

    private void HideContent(object sender, EventArgs e)
    {
        _contentStyle = "display: none;";
        _loadingStyle = "";
    }
}