using MapLab.Data.Entities;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Contracts
{
    public interface IMapsService
    {
        Task<string> GetMapAsync(string mapId);
        IEnumerable<Map>? GetMapsForProfile(string profileId, bool isCurrentProfile);
        Task CreateMapAsync(string name, string mapTemplateId, bool isPublic);
        Task UploadMapTemplateAsync(MapTemplate mapTemplate, IFormFile file);
        IQueryable<MapTemplate> GetMapTemplates(MapTemplateFiltersModel? filters = null);
        IEnumerable<MapTemplate> GetRecentMapTemplates();
        IEnumerable<MapTemplate> GetFeaturedMapTemplates();
    }
}
