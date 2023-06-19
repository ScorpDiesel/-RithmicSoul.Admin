using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Options;
using MudBlazor;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Core.Models;
using RithmicSoul.Admin.Core.Utilities;
using RithmicSoulSharedLibrary.Extensions;
using Winista.Mime;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsDataGridWithDetailRow<T> : RsDataGridBase<T> where T : class
{
    [Inject] IOptions<AppSettings> AppSettingsOptions { get; set; }
    [Parameter] public int[]? DetailRowColumns { get; set; }
    [Parameter] public RenderFragment<T>? DetailRowContent { get; set; }

    private AppSettings _appSettings;

    protected override async Task OnInitializedAsync()
    {
        Initialize();
        await FilterItemsAsync();
    }

    private void Initialize()
    {
        _appSettings = AppSettingsOptions.Value;
        TableName = typeof(T).Name.Replace("Dto", string.Empty).SplitCamelCase();
        _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
    }

    private async Task FilterItemsAsync()
    {
        if (FilterIds is null)
        {
            //_items = await ApIAdminService?.GetAllAsync();
        }
        else
        {
            var allResults = await ApiAdminService?.GetAllAsync();
            IList<T> filteredResults = new List<T>();
            foreach (var item in allResults)
            {
                var property = item.GetType().GetProperties().FirstOrDefault(p => p.Name == ColumnToFilter);
                var value = property?.GetValue(item, null);
              
                if (value is null) continue;
                if (FilterIds.Contains((int)value)) filteredResults?.Add(item);
            }

            //_items = await ApIAdminService.GetAllAsync();
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
            builder.AddAttribute(2, _appSettings.RsDataGridRenderFragmentTitleAttribute, propertyInfo.Name.SplitCamelCase());
            if (propertyInfo.Name == GroupBy) builder.AddAttribute(3, _appSettings.RsDataGridRenderFragmentGroupingAttribute, true);
            builder?.CloseComponent();
        };
    }

    private List<RenderFragment> CreateDetailRowContentFromColumn(object rowItem)
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

    //private List<RenderFragment> CreateDetailRowContentFromColumn(string elementType, string columnName, object rowItem, string? contentUrlTemplate = null)
    //{
    //    List<RenderFragment> fragments = new();
    //    var columnValue = GetColumnValueByColumnName(rowItem, columnName) ?? string.Empty;
    //    if (!string.IsNullOrEmpty(contentUrlTemplate)) columnValue = string.Format($"{_baseAddress}{contentUrlTemplate}", columnValue);

    //    var (attributeDictionary, htmlElement) = GetHtmlElementFromMimeType(elementType, columnValue);
    //    var fragment = CreateRenderFragment(attributeDictionary, htmlElement);
    //    fragments.Add(fragment);

    //    return fragments;
    //}

    private object? GetColumnValueByColumnName(object rowItem, string columnName)
    {
        var property = rowItem.GetType()
            .GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        return property?.GetValue(rowItem, null);
    }

    private List<RenderFragment> CreateDetailRowContent(string elementType, string content)
    {
        List<RenderFragment> fragments = new();
        var (attributeDictionary, htmlElement) = GetHtmlElementFromMimeType(elementType, content);
        var fragment = CreateRenderFragment(attributeDictionary, htmlElement);
        fragments.Add(fragment);

        return fragments;
    }

    private (Dictionary<string, object> attributeDictionary, string htmlElement) GetHtmlElementFromMimeType(string mimeType, object content)
    {
        (Dictionary<string, object> attributeDictionary, string htmlElement) elementTuple = (null, null);
        Dictionary<string, object> attrDict = new();

        switch (mimeType.ToLower())
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
}