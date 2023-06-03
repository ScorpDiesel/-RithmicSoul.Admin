using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Enums;
using RithmicSoul.Admin.Infrastructure.Services;
using RithmicSoul.Models.Survey.Dtos;
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

    private IEnumerable<PropertyInfo> _properties;
    private IEnumerable<T> _items;
    private MudDataGrid<T> thisDataGrid;
    private Dictionary<(string, int), string> RowHighlight = new();
    private AppSettings _appSettings;
    private DialogParameters _dialogParameters;
    private bool _isExpanded;
    protected string TableName;

    protected override async Task OnInitializedAsync()
    {
        SetFields();
        await FilterItemsAsync();
    }

    private void SetFields()
    {
        _appSettings = AppSettingsOptions.Value;
        TableName = typeof(T).Name.Replace("Dto", string.Empty);
        _dialogParameters = new DialogParameters
        {
            { "OnOperationCompleted", new EventCallback(this, RefreshAsync) }
        };
        _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
    }

    private async Task FilterItemsAsync()
    {
        if (FilterIds is null)
        {
            _items = await ApiService?.GetAllAsync();
        }
        else
        {
            var allResults = await ApiService?.GetAllAsync();
            IList<T> filteredResults = new List<T>();
            foreach (var item in allResults)
            {
                var property = item.GetType().GetProperties().FirstOrDefault(p => p.Name == ColumnToFilter);
                var value = property?.GetValue(item, null);
              
                if (value is null) continue;
                if (FilterIds.Contains((int)value)) filteredResults?.Add(item);
            }

            _items = await ApiService.GetAllAsync();
        }
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

    private RenderFragment CreateColumn(PropertyInfo propertyInfo)
    {
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
            builder.AddAttribute(1, _appSettings.RsDataGridRenderFragmentPropertyAttribute, lambdaExp);
            builder.AddAttribute(2, _appSettings.RsDataGridRenderFragmentTitleAttribute, propertyInfo.Name); 
            if (propertyInfo.Name == GroupBy) builder.AddAttribute(3, _appSettings.RsDataGridRenderFragmentGroupingAttribute, true);
            builder?.CloseComponent();
        };
    }

    private async Task CreateNewItemAsync<T>(DialogResult result)
    {
        if (!result.Canceled)
        {
            dynamic resultData;
            bool isSuccessful;

            if (result.Data.IsGenericList())
            {
                resultData = (List<T>)result.Data;
                isSuccessful = await ApiService?.BulkInsertAsync(resultData);
            }
            else
            {
                resultData = (T)result.Data;
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
                await RefreshAsync();
            }
            else
            {
                message = string.Format(_appSettings.ItemCreatedFailureMessageTemplate, TableName);
            }

            ShowSnackBar(isSuccessful, message);
        }
    }

    public async Task ShowNewItemDialogAsync()
    {
        var dialog = await DialogService?.ShowAsync(TDialog, $"New {TableName}", _dialogParameters)!;
        var result = await dialog.Result;
        await CreateNewItemAsync<T>(result);
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
        var dialog = await DialogService?.ShowAsync<DeleteItemDialog>(null, _dialogParameters)!;
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            dynamic dtoList = new List<T> { dto }; //TODO: Implement checkbox selection in each row to add to collection to be sent to BulkDelete
            var response = await ApiService?.BulkDeleteAsync(dtoList)!;
            string message;

            if (response)
            {
                message = string.Format(_appSettings.ItemDeletedSuccessMessageTemplate, TableName);
                await ApiService.GetAllAsync();
                StateHasChanged();
            }
            else
            {
                message = string.Format(_appSettings.ItemDeletedFailureMessageTemplate, TableName);
            }

            ShowSnackBar(response, message);
        }

        //ResetRowHighlight(dto);
        StateHasChanged();
    }

    public async Task ShowEditItemDialogAsync<T>(T dto)
    {
        SetRowHighlight(dto);
        _dialogParameters.Add("Model", dto);
        IDialogReference? dialog;

        if (TEditDialog is not null)
        {
            dialog = await DialogService?.ShowAsync(TEditDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, TableName), _dialogParameters)!;
        }
        else
        {
            dialog = await DialogService?.ShowAsync(TDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, TableName), _dialogParameters)!;
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

    public async Task RefreshAsync()
    {
        var items = await ApiService?.GetAllAsync();
        thisDataGrid.Items = items;
        StateHasChanged();
    }
}