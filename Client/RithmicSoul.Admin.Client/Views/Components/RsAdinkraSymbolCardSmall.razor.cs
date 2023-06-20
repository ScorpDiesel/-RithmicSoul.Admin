using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using RithmicSoul.Admin.Client.Pages.Surveys;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsAdinkraSymbolCardSmall : ComponentBase
{
    [Inject] private HttpClient _httpClient { get; set; }
    [Parameter] public int SymbolId { get; set; }
    [Parameter] public string Style { get; set; }
    [Parameter] public AdinkraSymbolDto AdinkraSymbol  { get; set; }

    private int _elevation = 1;
    private bool _showContent;
    private string _contentStyle;
    private string _imgSrcUrl;
    private string _audioSrcUrl;

    protected override void OnInitialized()
    {
        Initialize();
    }

    private void Initialize()
    {
        HideContent(null, null);
        var baseAddress = _httpClient.BaseAddress?.ToString();
        _audioSrcUrl = $"{baseAddress}v3/AdinkraSymbols/{SymbolId}";
        _imgSrcUrl = $"{baseAddress}v2/AdinkraSymbols/{SymbolId}/200";
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (_showContent) return;
        HideContent(null, null);
    }

    private void CardElevationUp() => _elevation = 7;

    private void CardElevationDown() => _elevation = 1;

    private void DisplayContent(EventArgs obj)
    {
        _contentStyle = "";
        _showContent = true;
    }

    private void HideContent(object sender, EventArgs e)
    {
        _contentStyle = "display: none;";
        _showContent = false;
    }
}