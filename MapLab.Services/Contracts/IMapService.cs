using MapLab.Data.Entities;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Contracts
{
    public interface IMapService
    {
        Task<string> GetMapAsync(string mapId);
        Task<IEnumerable<Map>?> GetMapsForProfile(string profileId);
        Task CreateMapAsync(string name, string mapTemplateId);
        Task UploadMapTemplateAsync(MapTemplate mapTemplate, IFormFile file);
        IQueryable<MapTemplate> GetMapTemplates(MapTemplateFiltersModel? filters = null);
        IQueryable<MapTemplate> GetRecentMapTemplates();
        IQueryable<MapTemplate> GetFeaturedMapTemplates();
    }
}
