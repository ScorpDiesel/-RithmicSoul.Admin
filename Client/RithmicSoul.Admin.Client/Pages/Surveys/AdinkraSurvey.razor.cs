using Microsoft.AspNetCore.Components;
using RithmicSoul.Models.Survey.Dtos;
using RithmicSoulDatabaseLibrary.Interfaces;

namespace RithmicSoul.Admin.Client.Pages.Surveys;

public partial class AdinkraSurvey : ComponentBase
{
    [Inject] private IService<AdinkraSymbolDto> AdinkraSymbolService { get; set; }

    private IEnumerable<AdinkraSymbolDto>? _allSymbols;
    private IEnumerable<AdinkraSymbolDto>? CurrentSymbols => _allSymbols?.Skip(_currentPage * _pageSize).Take(_pageSize);

    private int _pageSize = 10;
    private int _currentPage = 0;
    private HashSet<int> _symbolLikes = new();

    protected override async Task OnInitializedAsync()
    {
        _allSymbols = await AdinkraSymbolService.GetAllAsync();
    }

    private bool HasPreviousPage => _currentPage > 0;
    private bool HasNextPage => (_currentPage + 1) * _pageSize < _allSymbols?.Count();

    private void PreviousPage()
    {
        if (HasPreviousPage) _currentPage--;
    }

    private void NextPage()
    {
        if (HasNextPage) _currentPage++;
    }

    public void TallySymbolLikes(bool isSelected, int symbolId)
    {
        Console.WriteLine(isSelected);
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