using Refit;
using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces;

public interface IAdinkraSymbolRepository<T> : IAdminRepository<T> where T : class
{
    [Get("")] Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null);
    [Get("")] Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id);
}