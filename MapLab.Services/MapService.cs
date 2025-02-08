using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MapLab.Services
{
    public class MapService : IMapService
    {
        private readonly IDeletableEntityRepository<Map> _mapRepository;
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;

        private readonly IFileStorageManager _fileStorageManager;
        private readonly ProfileManager<Profile> _profileManager;

        private readonly IProfileService _profileService;

        public MapService(IDeletableEntityRepository<Map> mapRepository, IDeletableEntityRepository<MapTemplate> mapTemplateRepository, IFileStorageManager fileStorageManager, ProfileManager<Profile> profileManager, IProfileService profileService)
        {
            _mapRepository = mapRepository;
            _mapTemplateRepository = mapTemplateRepository;

            _fileStorageManager = fileStorageManager;
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

        public async Task<string> GetMapAsync(string mapId)
        {
            var mapEntity = await _mapRepository.AllWithIncludes(q => q
                .Include(q => q.Template))
                .FirstOrDefaultAsync(m => m.Id == mapId);

            //TO BE ENABLED ONCE FILE STORAGE SYSTEM IS COMPLETE
            /*var template = await _fileStorageManager.GetFileAsync(mapEntity?.Template?.File?.Path);
            var map = await _fileStorageManager.GetFileAsync(mapEntity?.File?.Path);*/

            var template = await _fileStorageManager.GetFileAsync("C:\\Users\\ITBP\\Desktop\\maptest\\template.json");
            var map = await _fileStorageManager.GetFileAsync("C:\\Users\\ITBP\\Desktop\\maptest\\map.json");

            string templateJson = Encoding.UTF8.GetString(template);
            string mapJson = Encoding.UTF8.GetString(map);

            JObject templateJsonObject = JObject.Parse(templateJson);
            JObject mapJsonObject = JObject.Parse(mapJson);

            JObject completeMapJsonObject = new JObject(templateJsonObject);
            ((JArray)completeMapJsonObject["features"]).Merge(mapJsonObject["features"]);

            return completeMapJsonObject.ToString();
        }

        public IQueryable<MapTemplate> GetAllMapTemplates() => _mapTemplateRepository.AllWithIncludes(q => q
            .Include(mt => mt.Profile)
        );

        public IQueryable<MapTemplate> GetMapTemplates(string name) => GetAllMapTemplates().Where(t => EF.Functions.Like(t.Name, $"%{name}%"));

        public async Task UploadMapTemplateAsync(MapTemplate mapTemplate)
        {
            await _mapTemplateRepository.AddAsync(mapTemplate);
            await _mapTemplateRepository.SaveChangesAsync();
        }
    }
}
