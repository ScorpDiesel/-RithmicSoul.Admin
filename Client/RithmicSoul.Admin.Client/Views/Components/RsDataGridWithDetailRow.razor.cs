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
    [Parameter] public RenderFragment<T>? DetailRowContent { get; set; }
}