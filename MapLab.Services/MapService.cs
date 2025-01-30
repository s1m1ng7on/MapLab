using MapLab.Data.Entities;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Services
{
    public class MapService : IMapService
    {
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;

        public MapService(IDeletableEntityRepository<MapTemplate> mapTemplateRepository)
        {
            _mapTemplateRepository = mapTemplateRepository;
        }

        public IQueryable<MapTemplate> GetAllMapTemplates() => _mapTemplateRepository.AllAsNoTracking();

        public IQueryable<MapTemplate> GetMapTemplates(string name) => GetAllMapTemplates().Where(t => EF.Functions.Like(t.Name, $"%{name}%"));

        public async Task UploadMapTemplateAsync(MapTemplate mapTemplate)
        {
            await _mapTemplateRepository.AddAsync(mapTemplate);
            await _mapTemplateRepository.SaveChangesAsync();
        }
    }
}
