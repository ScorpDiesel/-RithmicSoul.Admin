using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class AdinkraSymbolCard : ComponentBase
{
    [Parameter] public string ImgSrc { get; set; }
    [Parameter] public string AudioSrc { get; set; }
    [Parameter] public string SymbolName { get; set; }
    [Parameter] public string SymbolTranslation { get; set; }
    [Parameter] public string SymbolMeaning { get; set; }
    [Parameter] public int SymbolId { get; set; }
    [Parameter] public HashSet<int> SymbolLikes { get; set; }
    [Parameter] public EventCallback<bool> OnSymbolClicked { get; set; }
    
    private bool _selected;

    protected override void OnAfterRender(bool firstRender)
    {
        _selected = SymbolLikes.Contains(SymbolId);
    }

    public async Task SymbolClickedAsync()
    {
        _selected = !_selected;
        await OnSymbolClicked.InvokeAsync(_selected);
    }

    private Color IconButtonColor => _selected ? Color.Error : Color.Default;
}