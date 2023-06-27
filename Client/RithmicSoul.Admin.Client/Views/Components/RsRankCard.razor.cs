using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace RithmicSoul.Admin.Client.Views.Components;

public partial class RsRankCard : ComponentBase
{
    [Parameter] public string RankListHeader { get; set; }
    [Parameter] public List<string> Items { get; set; }

    private string _rankDropZone = "RankDropZone";

    private void ItemUpdated(MudItemDropInfo<string> dropItem)
    {
        //dropItem.Item.Selector = dropItem.DropzoneIdentifier;
    }
}