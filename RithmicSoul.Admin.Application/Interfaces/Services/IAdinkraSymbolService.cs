using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces.Services;

public interface IAdinkraSymbolService<T> : IAdminService<T> where T : class
{
    Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null);
    Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id);
}