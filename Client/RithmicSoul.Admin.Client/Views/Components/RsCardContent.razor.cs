using Microsoft.AspNetCore.Components;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsCardContent : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    private string _prevContent;

    protected override bool ShouldRender()
    {
        var currentContent = ChildContent?.ToString();
        if (_prevContent != currentContent)
        {
            _prevContent = currentContent;
            return true;
        }
        return false;
    }
}