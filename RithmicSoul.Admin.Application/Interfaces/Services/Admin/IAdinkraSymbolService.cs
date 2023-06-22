using RithmicSoul.Models.Survey.Dtos;

namespace RithmicSoul.Admin.Application.Interfaces.Services.Admin;

public interface IAdinkraSymbolService
{
    Task<IEnumerable<AdinkraSymbolDto>> GetImageDataByIdAsync(int id, int? w = null, int? h = null);
    Task<IEnumerable<AdinkraSymbolDto>> GetAudioDataByIdAsync(int id);
}