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
    [Parameter] public string? TableName { get; set; }
    [Parameter] public IService<T>? ApiService { get; set; }
    [Parameter] public bool CanGroup { get; set; }
    [Parameter] public bool GroupExpanded { get; set; }
    [Parameter] public string? GroupBy { get; set; }
    [Parameter] public bool MultiSelection { get; set; }
    [Parameter] public int[]? ColumnsToHide { get; set; }
    [Parameter] public bool CanEdit { get; set; }
    [Parameter] public bool CanCreate { get; set; }
    [Parameter] public bool CanDelete { get; set; }
    [Parameter] public Type? TEditDialog { get; set; }
    [Parameter] public Type? TDialog { get; set; }

    private IEnumerable<PropertyInfo> _properties;
    private IEnumerable<T> _items;
    private MudDataGrid<T> thisDataGrid;
    private Dictionary<(string, int), string> RowHighlight = new();
    private AppSettings _appSettings;

    protected override async Task OnInitializedAsync()
    {
        _appSettings = AppSettingsOptions.Value;
        _items = await ApiService.GetAllAsync();
        _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
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
            builder.CloseComponent();
        };
    }

    private async Task CreateNewItemAsync<T>(DialogResult result, string tableName)
    {
        if (!result.Canceled)
        {
            dynamic resultData;
            bool isSuccessful;

            if (result.Data.IsGenericList())
            {
                resultData = (List<T>)result.Data;
                isSuccessful = await ApiService.BulkInsertAsync(resultData);
            }
            else
            {
                resultData = (T)result.Data;
                var id = await ApiService.InsertForIdAsync(resultData);
                isSuccessful = id is int;
            }

            string message;

            if (isSuccessful)
            {
                message = string.Format(_appSettings.ItemCreatedSuccessMessageTemplate, tableName);
                await RefreshAsync();
                StateHasChanged();
            }
            else
            {
                message = string.Format(_appSettings.ItemCreatedFailureMessageTemplate, tableName);
            }

            ShowSnackBar(isSuccessful, message);
        }
    }

    public async Task CreateAsync()
    {
        var tableName = typeof(T).Name.Replace("Dto", string.Empty);
        var dialog = await DialogService.ShowAsync(TDialog, $"New {tableName}");
        var result = await dialog.Result;

        await CreateNewItemAsync<T>(result, tableName);
    }

    private async Task UpdateItemAsync(T dto)
    {
        SetRowHighlight(dto);
        var dtoName = dto.GetType().Name;
        var tableName = dtoName.Replace("Dto", string.Empty);
        var response = await ApiService.UpdateAsync(dto);
        ResetRowHighlight(dto);
        StateHasChanged();
        ShowSnackBar(response, string.Format(_appSettings.ItemUpdatedSuccessMessageTemplate, tableName));
    }

    public async Task DeleteAsync<T>(T dto)
    {
        SetRowHighlight(dto);
        var dialog = await DialogService.ShowAsync<DeleteItemDialog>(null);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var dtoName = dto.GetType().Name;
            var tableName = dtoName.Replace("Dto", string.Empty);
            dynamic dtoList = new List<T> { dto }; //TODO: Implement checkbox selection in each row to add to collection to be sent to BulkDelete
            var response = await ApiService.BulkDeleteAsync(dtoList);
            string message;

            if (response)
            {
                message = string.Format(_appSettings.ItemDeletedSuccessMessageTemplate, tableName);
                await ApiService.GetAllAsync();
                StateHasChanged();
            }
            else
            {
                message = string.Format(_appSettings.ItemDeletedFailureMessageTemplate, tableName);
            }

            ShowSnackBar(response, message);
        }

        //ResetRowHighlight(dto);
        StateHasChanged();
    }

    public async Task EditAsync<T>(T dto)
    {
        SetRowHighlight(dto);
        var parameters = new DialogParameters { { "Model", dto } };
        var tableName = typeof(T).Name.Replace("Dto", string.Empty);
        IDialogReference? dialog;

        if (TEditDialog is not null)
        {
            dialog = await DialogService.ShowAsync(TEditDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, tableName), parameters);
        }
        else
        {
            dialog = await DialogService.ShowAsync(TDialog, string.Format(_appSettings.EditTableDialogMessageTemplate, tableName), parameters);
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
        return RowHighlight.TryGetValue((dto.GetType().Name, dto.GetHashCode()), out string bgColor) ? string.Format(_appSettings.RowHighlightBgColorTemplate, bgColor) : string.Empty;
    }

    protected void ResetRowHighlight(object dto)
    {
        RowHighlight.Remove((dto.GetType().Name, dto.GetHashCode()));
    }

    void ShowSnackBar(bool isSuccess, string message)
    {
        Snackbar.Clear();
        Snackbar.Add(message, isSuccess ? Severity.Success : Severity.Error);
    }

    public async Task RefreshAsync()
    {
        var items = await ApiService.GetAllAsync();
        thisDataGrid.Items = items;
        StateHasChanged();
    }
}