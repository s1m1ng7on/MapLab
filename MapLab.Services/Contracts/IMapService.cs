using MapLab.Data.Entities;

namespace MapLab.Services.Contracts
{
    public interface IMapService
    {
        IQueryable<MapTemplate> GetAllMapTemplates();
        IQueryable<MapTemplate> GetMapTemplates(string name);
        Task UploadMapTemplateAsync(MapTemplate mapTemplate);
    }
}
