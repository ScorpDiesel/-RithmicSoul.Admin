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
    [Parameter] public bool? UseBase64Data { get; set; }
    [Parameter] public AdinkraSurvey Parent { get; set; }

    private int _maxMeaningLength = 50;
    private int _maxTranslationLength = 35;
    private int _maxNameLength = 25;
    private int _elevation = 1;
    private bool _isLoaded;
    private string _contentStyle;
    private string _loadingStyle;

    protected override void OnInitialized()
    {
        _isLoaded = true;
        Parent.NewPageAsync += NewPagAsync;
        HideContent();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (_isLoaded) return;
        HideContent();
    }

    public async Task SymbolClickedAsync()
    {
        ShowContent();
        IsSelected = !IsSelected;
        await OnSymbolClicked.InvokeAsync(IsSelected);
    }

    private Color IconButtonColor => IsSelected ? Color.Error : Color.Default;

    private bool IsStringLengthTooLong(string str, int maxLength) => str.Length > maxLength;

    private void CardElevationUp() => _elevation = 7;

    private void CardElevationDown() => _elevation = 1;

    private void ImageLoad(EventArgs obj)
    {
        ShowContent();
    }

    private void ShowContent()
    {
        _contentStyle = "";
        _loadingStyle = "display: none;";
        _isLoaded = true;
    }

    private async Task NewPagAsync(object? sender, EventArgs e)
    {
        HideContent();
    }

    private void HideContent()
    {
        _contentStyle = "display: none;";
        _loadingStyle = "";
    }

    public void Dispose()
    {
        Parent.NewPageAsync -= NewPagAsync;
    }

}