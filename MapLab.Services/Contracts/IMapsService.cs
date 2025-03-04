using MapLab.Data.Entities;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MapLab.Services.Contracts
{
    public interface IMapsService
    {
        Task<Map> GetMapAsync(string id);
        Task<(string, JObject)> GetMapJsonAsync(Map map);
        IEnumerable<Map>? GetMapsForProfile(string profileId, bool isCurrentProfile);
        Task CreateMapAsync(string name, string mapTemplateId, bool isPublic);
        Task DeleteMapAsync(string id);
        Task SaveMapAsync(string Id, string updatedMapJson);
        Task UploadMapTemplateAsync(MapTemplate mapTemplate, IFormFile file);
        IQueryable<MapTemplate> GetMapTemplates(MapTemplateFiltersModel? filters = null);
        IEnumerable<MapTemplate> GetRecentMapTemplates();
        IEnumerable<MapTemplate> GetFeaturedMapTemplates();
    }
}
