using MapLab.Data.Entities;

namespace MapLab.Services.Contracts
{
    public interface IMapService
    {
        Task<IEnumerable<Map>?> GetMapsForProfile(string profileUserName);
        IQueryable<MapTemplate> GetAllMapTemplates();
        IQueryable<MapTemplate> GetMapTemplates(string name);
        Task UploadMapTemplateAsync(MapTemplate mapTemplate);
    }
}
