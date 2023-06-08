using Microsoft.AspNetCore.Components;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class AdinkraSurvey : ComponentBase
{
    [Inject] private HttpClient _httpClient { get; set; }
    [Inject] private IService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }

    private IEnumerable<AdinkraSymbolDto>? _allSymbols;
    private IEnumerable<AdinkraSymbolDto>? CurrentSymbols => _allSymbols?.Skip(_currentPage * _pageSize).Take(_pageSize); [CascadingParameter]
    public EventCallback HideMenus { get; set; }

    private int _pageSize = 10;
    private int _currentPage;
    private HashSet<int> _symbolLikes = new();
    private string? _baseAddress;

    protected override async Task OnInitializedAsync()
    {
        await HideMenus.InvokeAsync();
        _baseAddress = _httpClient.BaseAddress?.ToString();
        _allSymbols = await AdinkraSymbolService.GetAllAsync();
    }

    private bool HasPreviousPage => _currentPage > 0;
    private bool HasNextPage => (_currentPage + 1) * _pageSize < _allSymbols?.Count();

    private void PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        _currentPage--;
    }

    private void NextPageAsync()
    {
        if (!HasNextPage) return;
        _currentPage++;
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
}