using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.Threading;
using MudBlazor;
using RithmicSoul.Admin.Application.Interfaces.Services.Admin;
using RithmicSoul.Admin.Client.Views.Dialogs;
using RithmicSoul.Admin.Infrastructure.Services;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoul.Models.Survey.Models;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class AdinkraSurvey : ComponentBase
{
    [Inject] IDialogService? DialogService { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] private HttpClient _httpClient { get; set; }
    [Inject] private IAdminService<AdinkraSymbolDto> adinkraSymbolService { get; set; }
    [Inject] private SurveyResponsesService surveyResponsesService { get; set; }

    private IEnumerable<AdinkraSymbolDto>? _allSymbols;
    private IEnumerable<AdinkraSymbolDto>? CurrentSymbols => _allSymbols?.Skip(_currentPage * _pageSize).Take(_pageSize);
    [CascadingParameter] public EventCallback<bool> HideMenus { get; set; }
    [Parameter] public Guid Id { get; set; }

    public event AsyncEventHandler NewPageAsync;

    private readonly int _pageSize = 10;
    private int _currentPage;
    private HashSet<int> _symbolLikes = new();
    private string? _baseAddress;
    private bool _endOfSurveyReached;
    //private bool _isSaveButtonDisabled = true;

    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("setPlaybackRate", 0.75);
    }


    private async Task InitializeAsync()
    {
        await HideMenus.InvokeAsync(true);
        _baseAddress = _httpClient.BaseAddress?.ToString();
        _allSymbols = await adinkraSymbolService.GetAllAsync();
    }

    private bool HasPreviousPage => _currentPage > 0;
    private bool HasNextPage => (_currentPage + 1) * _pageSize < _allSymbols?.Count();

    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        _currentPage--;
        await NewPageAsync.InvokeAsync(this, EventArgs.Empty);
    }

    private async Task NextPageAsync()
    {
        if (!HasNextPage)
        {
            _endOfSurveyReached = true;
            return;
        }
        _currentPage++;
        await NewPageAsync.InvokeAsync(this, EventArgs.Empty);
    }

    public void TallySymbolLikes(bool isSelected, int symbolId)
    {
        if (isSelected)
        {
            _symbolLikes.Add(symbolId);
        }
        else
        {
            _symbolLikes.Remove(symbolId);
        }
    }

    private async Task SaveSurveyAsync()
    {
        var contentText = _endOfSurveyReached
            ? "Save the survey?"
            : "Looks like you haven't completed the survey.\r\nSave anyway?";
        var parameters = new DialogParameters
        {
            { "ContentText", contentText },
            { "CloseButtonText", "Yes" },
            { "CancelButtonText", "No" },
            { "Style", "min-width:300px" },
            { "Color", Color.Info }
        };
        var dialog = await DialogService?.ShowAsync<ActionDialog>("Save", parameters)!;
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            if (!_symbolLikes.Any()) return;
            var symbolNames = _allSymbols.Where(a => _symbolLikes.Any(s => s == a.SymbolId)).Select(r => r.SymbolName).ToList();
            var responses = new SurveyResponses
            {
                UserId = Id,
                Answers = symbolNames
            };

            var isSuccess = await surveyResponsesService.UpdateResponsesAsync(responses);
        }
    }
}