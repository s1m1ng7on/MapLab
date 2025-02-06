using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Services
{
    public class MapService : IMapService
    {
        private readonly IDeletableEntityRepository<Map> _mapRepository;
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;

        private readonly ProfileManager<Profile> _profileManager;

        private readonly IProfileService _profileService;

        public MapService(IDeletableEntityRepository<Map> mapRepository, IDeletableEntityRepository<MapTemplate> mapTemplateRepository, ProfileManager<Profile> profileManager, IProfileService profileService)
        {
            _mapRepository = mapRepository;
            _mapTemplateRepository = mapTemplateRepository;

            _profileManager = profileManager;

            _profileService = profileService;
        }

        public async Task<IEnumerable<Map>?> GetMapsForProfile(string profileUserName)
        {
            var selectedProfileId = string.IsNullOrEmpty(profileUserName)
                    ? _profileService.GetProfileId()
                    : (await _profileManager.FindByNameAsync(profileUserName))?.Id;

            return await _mapRepository.AllWithIncludes(q => q
                .Where(m => m.ProfileId == selectedProfileId)
                .Include(m => m.Profile)
                .Include(m => m.Template)
            ).ToListAsync();
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
