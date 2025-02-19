using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MapLab.Services
{
    public class MapsService : IMapsService
    {
        private readonly IDeletableEntityRepository<Map> _mapRepository;
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;

        private readonly IMemoryCache _memoryCache;

        private readonly IFileStorageManager _fileStorageManager;

        private readonly IProfileService _profileService;

        private const string FeaturedMapTemplatesCacheKey = "FeaturedMapTemplates";

        public MapsService(IDeletableEntityRepository<Map> mapRepository, IDeletableEntityRepository<MapTemplate> mapTemplateRepository, IMemoryCache memoryCache, IFileStorageManager fileStorageManager, IProfileService profileService)
        {
            _mapRepository = mapRepository;
            _mapTemplateRepository = mapTemplateRepository;

            _memoryCache = memoryCache;

            _fileStorageManager = fileStorageManager;

            _profileService = profileService;
        }

        public IEnumerable<Map>? GetMapsForProfile(string profileId)
            => _mapRepository.All()
                .Where(m => m.ProfileId == profileId)
                .Include(m => m.Profile)
                .Include(m => m.Template);

        public async Task<string> GetMapAsync(string mapId)
        {
            var mapEntity = await _mapRepository.All()
                .Include(q => q.Template)
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

        public IQueryable<MapTemplate> GetMapTemplates(MapTemplateFiltersModel? filters = null)
        {
            var query = _mapTemplateRepository.All()
                .Include(mt => mt.Profile)
                .AsQueryable();

            if (filters != null)
            {
                query = query.Where(mt =>
                    (string.IsNullOrEmpty(filters.SearchQuery) || EF.Functions.Like(mt.Name, $"%{filters.SearchQuery}%")) &&
                    (!filters.Region.HasValue || mt.Region == filters.Region) &&
                    (!filters.ByMapLab || mt.Profile!.UserName == "MapLab")
                );
            }

            return query;
        }

        public IEnumerable<MapTemplate> GetRecentMapTemplates()
            => _mapTemplateRepository.All()
                .Include(mt => mt.Maps)
                .Where(mt => mt.Maps.Any(m => m.ProfileId == _profileService.GetProfileId()))
                .OrderByDescending(mt => mt.CreatedOn);

        public IEnumerable<MapTemplate> GetFeaturedMapTemplates()
        {
            if (!_memoryCache.TryGetValue(FeaturedMapTemplatesCacheKey, out IEnumerable<MapTemplate> cachedMapTemplates))
            {
                //TO BE CHANGED
                cachedMapTemplates = GetMapTemplates().ToList();

                var nextMonday = DateTime.Now.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek + 7) % 7).Date;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(nextMonday);

                _memoryCache.Set(FeaturedMapTemplatesCacheKey, cachedMapTemplates, cacheEntryOptions);
            }

            return cachedMapTemplates;
        }


        public async Task CreateMapAsync(string name, string mapTemplateId)
        {
            Map newMap = new Map()
            {
                Name = name,
                TemplateId = mapTemplateId,
                ProfileId = _profileService.GetProfileId()
            };

            await _mapRepository.AddAsync(newMap);
            await _mapRepository.SaveChangesAsync();
        }

        public async Task UploadMapTemplateAsync(MapTemplate mapTemplate, IFormFile file)
        {
            await _mapTemplateRepository.AddAsync(mapTemplate);
            await _fileStorageManager.SaveFileAsync(file, "MapTemplates", "File", mapTemplate.Id!);
            await _mapTemplateRepository.SaveChangesAsync();
        }
    }
}
