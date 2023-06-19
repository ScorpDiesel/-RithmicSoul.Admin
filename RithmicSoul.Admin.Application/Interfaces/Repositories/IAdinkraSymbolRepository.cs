using Refit;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces;

public interface IAdinkraSymbolRepository<T> : IAdminRepository<T> where T : class
{
    [Get("/v2/AdinkraSymbols/{id}/{w}/{h}")] Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null);
    [Get("/v3/AdinkraSymbols/{id}")] Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id);
}