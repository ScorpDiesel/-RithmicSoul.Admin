using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Configuration;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Utilities;
using RithmicSoulDatabaseLibrary.Interfaces;
using RithmicSoulSharedLibrary.Extensions;
using Winista.Mime;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsDataGridWithDetailRow<T> : ComponentBase where T : class
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
    [Parameter] public int[]? DetailRowColumns { get; set; }

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
        TableName = typeof(T).Name.Replace("Dto", string.Empty).SplitCamelCase();
        _dialogParameters = new DialogParameters
        {
            //{ "OnOperationCompleted", new EventCallback(this, RefreshAsync) }
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

    private List<RenderFragment> CreateDetailRowContent(object rowItem)
    {
        List<RenderFragment> fragments = new();

        foreach (var item in rowItem.GetType().GetProperties().Select((value, index) => new { index, value }))
        {
            if (DetailRowColumns is not null && !DetailRowColumns.Contains(item.index + 1)) continue;

            var property = item.value;
            var contentValue = property.GetValue(rowItem);
            var mimeType = Utilities.GetMimeTypeString((string)contentValue);
            if (mimeType is null) continue;

            var (attributeDictionary, htmlElement) = GetHtmlElementFromMimeType(mimeType, contentValue);
            var fragment = CreateRenderFragment(attributeDictionary, htmlElement);
            fragments.Add(fragment);
        }

        return fragments;
    }

    private (Dictionary<string, object> attributeDictionary, string htmlElement) GetHtmlElementFromMimeType(string mimeType, object content)
    {
        (Dictionary<string, object> attributeDictionary, string htmlElement) elementTuple = (null, null);
        Dictionary<string, object> attrDict = new();

        switch (mimeType)
        {
            case "image":
                var element = "img";
                var imageSrc = content;
                attrDict["src"] = imageSrc;
                attrDict["width"] = "200";
                elementTuple = (attrDict, element);
                break;
            case "audio":
                element = "audio";
                var audioSrc = content;
                attrDict["src"] = audioSrc;
                attrDict["controls"] = "controls";
                elementTuple = (attrDict, element);
                break;
            case "text":
                break;
        }

        return elementTuple;
    }

    private RenderFragment? CreateRenderFragment(Dictionary<string, object>? attributeDictionary = null, string? htmlElement = null, string? textContent = null)
    {
        if (attributeDictionary is null && htmlElement is null && textContent is null) return null;

        if (htmlElement is null && textContent is null) 
            throw new ArgumentNullException($"{nameof(htmlElement)} and {nameof(textContent)} cannot both be null.");

        if (htmlElement is null && attributeDictionary is not null) throw new ArgumentNullException(nameof(htmlElement));

        return builder =>
        {
            var seq = 0;
            if (htmlElement is not null) builder.OpenElement(seq, htmlElement);

            if (attributeDictionary is not null)
                foreach (var attribute in attributeDictionary)
                {
                    var name = attribute.Key;
                    var value = attribute.Value;
                    builder.AddAttribute(++seq, name, value);
                }

            if (textContent is not null) builder.AddContent(++seq, textContent);
            builder.CloseComponent();
        };
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