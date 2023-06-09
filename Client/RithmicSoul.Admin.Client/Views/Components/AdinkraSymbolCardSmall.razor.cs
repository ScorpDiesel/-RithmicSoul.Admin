using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using RithmicSoul.Admin.Client.Pages.Surveys;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class AdinkraSymbolCardSmall : ComponentBase
{
    [Parameter] public int SymbolId { get; set; }
    [Parameter] public string ImgSrcUrl { get; set; }
    [Parameter] public string AudioSrcUrl { get; set; }
    [Parameter] public string Style { get; set; }
    
    private int _elevation = 1;
    private bool _isLoaded;
    private string _contentStyle;
    private string _loadingStyle;

    protected override void OnInitialized()
    {
        _isLoaded = true;
        HideContent(null, null);
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