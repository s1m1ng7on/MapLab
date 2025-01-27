using MapLab.Data.Entities;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Services
{
    public class MapService : IMapService
    {
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;
        private readonly IFileStorageService _fileStorageService;

        public MapService(IDeletableEntityRepository<MapTemplate> mapTemplateRepository, IFileStorageService fileStorageService)
        {
            _mapTemplateRepository = mapTemplateRepository;
            _fileStorageService = fileStorageService;
        }

        public IQueryable<MapTemplate> GetAllMapTemplates() => _mapTemplateRepository.AllAsNoTracking();

        public IQueryable<MapTemplate> GetMapTemplates(string name) => GetAllMapTemplates().Where(t => EF.Functions.Like(t.Name, $"%{name}%"));

        public async Task UploadMapTemplateAsync(MapTemplate mapTemplate)
        {
            await _mapTemplateRepository.AddAsync(mapTemplate);
            await _mapTemplateRepository.SaveChangesAsync();

            await _fileStorageService.SaveFileAsync(mapTemplate.File, typeof(MapTemplate).Name + "s", mapTemplate.Id, nameof(mapTemplate.File));
        }
    }
}
