using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using RithmicSoul.Admin.Client.Pages.Surveys;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class AdinkraSymbolCard : ComponentBase
{
    [Parameter] public int SymbolId { get; set; }
    [Parameter] public string SymbolName { get; set; }
    [Parameter] public string SymbolTranslation { get; set; }
    [Parameter] public string SymbolMeaning { get; set; }
    [Parameter] public string ImgSrcUrl { get; set; }
    [Parameter] public string AudioSrcUrl { get; set; }
    [Parameter] public string Style { get; set; }
    [Parameter] public EventCallback<bool> OnSymbolClicked { get; set; }
    [Parameter] public bool IsSelected { get; set; }

    private int _maxMeaningLength = 50;
    private int _maxTranslationLength = 35;
    private int _maxNameLength = 25;
    private int _elevation = 1;
    private bool _isLoading = true;
    private string _contentStyle;
    private string _loadingStyle;

    protected override void OnInitialized()
    {
        _contentStyle = "display: none;";
        _loadingStyle = "";
    }

    protected override void OnAfterRender(bool firstRender)
    {
        _contentStyle = "display: none;";
        _loadingStyle = "";
    }

    public async Task SymbolClickedAsync()
    {
        IsSelected = !IsSelected;
        await OnSymbolClicked.InvokeAsync(IsSelected);
    }

    private Color IconButtonColor => IsSelected ? Color.Error : Color.Default;

    private bool IsStringLengthTooLong(string str, int maxLength)
    {
        return str.Length > maxLength;
    }

    private void CardElevationUp()
    {
        DisplayContent(null);
        _elevation = 7;
    }

    private void CardElevationDown()
    {
        DisplayContent(null);
        _elevation = 1;
    }

    private void DisplayContent(EventArgs obj)
    {
        _contentStyle = "";
        _loadingStyle = "display: none;";
    }
}