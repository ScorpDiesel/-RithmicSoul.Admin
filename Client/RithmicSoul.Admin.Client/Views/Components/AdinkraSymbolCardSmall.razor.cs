using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using RithmicSoul.Admin.Client.Pages.Surveys;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class AdinkraSymbolCardSmall : ComponentBase
{
    [Inject] private HttpClient _httpClient { get; set; }
    [Parameter] public int SymbolId { get; set; }
    [Parameter] public string Style { get; set; }
    [Parameter] public AdinkraSymbolDto AdinkraSymbol  { get; set; }

    private int _elevation = 1;
    private bool _isLoaded;
    private string _contentStyle;
    private string _loadingStyle;
    private string _imgSrcUrl;
    private string _audioSrcUrl;

    protected override void OnInitialized()
    {
        Initialize();
    }

    private void Initialize()
    {
        _isLoaded = true;
        HideContent(null, null);
        var baseAddress = _httpClient.BaseAddress?.ToString();
        _audioSrcUrl = $"{baseAddress}v3/AdinkraSymbols/{SymbolId}";
        _imgSrcUrl = $"{baseAddress}v2/AdinkraSymbols/{SymbolId}/200";
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (_isLoaded) return;
        HideContent(null, null);
    }

    private void CardElevationUp() => _elevation = 7;

    private void CardElevationDown() => _elevation = 1;

    private void DisplayContent(EventArgs obj)
    {
        _contentStyle = "";
        _loadingStyle = "display: none;";
        _isLoaded = true;
    }

    private void HideContent(object sender, EventArgs e)
    {
        _contentStyle = "display: none;";
        _loadingStyle = "";
    }
}