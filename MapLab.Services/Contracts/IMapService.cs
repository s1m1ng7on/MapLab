using MapLab.Data.Entities;
using MapLab.Shared.Models.FilterModels;

namespace MapLab.Services.Contracts
{
    public interface IMapService
    {
        Task<string> GetMapAsync(string mapId);
        Task<IEnumerable<Map>?> GetMapsForProfile(string profileUserName);
        IQueryable<MapTemplate> GetAllMapTemplates();
        IQueryable<MapTemplate> GetMapTemplates(string name);
        Task CreateMapAsync(string name, string mapTemplateId);
        Task UploadMapTemplateAsync(MapTemplate mapTemplate);
        IQueryable<MapTemplate> GetMapTemplates(MapTemplateFiltersModel filters);
    }
}
