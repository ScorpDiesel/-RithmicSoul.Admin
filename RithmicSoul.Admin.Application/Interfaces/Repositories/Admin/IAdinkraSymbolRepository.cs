using Refit;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces.Repositories.Admin;

public interface IAdinkraSymbolRepository
{
    [Get("/v2/AdinkraSymbols/{id}/{w}/{h}")] Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null);
    [Get("/v3/AdinkraSymbols/{id}")] Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id);
}