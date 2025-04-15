using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace MapLab.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;
        private readonly IMapper _mapper;
        private readonly IProfileService _profileService;
        private readonly IMemoryCache _memoryCache;
        private readonly IFileStorageManager _fileStorageManager;

        private const string FeaturedMapTemplatesCacheKey = "FeaturedMapTemplates";

        public TemplatesService(IDeletableEntityRepository<MapTemplate> mapTemplateRepository, IMapper mapper, IProfileService profileService, IMemoryCache memoryCache, IFileStorageManager fileStorageManager)
        {
            _mapTemplateRepository = mapTemplateRepository;
            _mapper = mapper;
            _profileService = profileService;
            _memoryCache = memoryCache;
            _fileStorageManager = fileStorageManager;
        }

        public async Task<MapTemplateDto> GetMapTemplateAsync(string id)
        {
            var newsArticle = await _mapTemplateRepository.All()
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<MapTemplateDto>(newsArticle);
        }

        public IEnumerable<MapTemplateDto> GetMapTemplates(MapTemplateFiltersModel? filters = null)
        {
            var mapTemplates = _mapTemplateRepository.All()
                .Include(mt => mt.Profile)
                .AsQueryable();

            if (filters != null)
            {
                mapTemplates = mapTemplates.Where(mt =>
                    (string.IsNullOrEmpty(filters.SearchQuery) || EF.Functions.Like(mt.Name, $"%{filters.SearchQuery}%")) &&
                    (!filters.Region.HasValue || mt.Region == filters.Region) &&
                    (!filters.ByMapLab || mt.Profile!.UserName == "MapLab")
                );
            }

            return _mapper.Map<IEnumerable<MapTemplateDto>>(mapTemplates);
        }

        public IEnumerable<MapTemplateDto> GetRecentMapTemplates()
        {
            var mapTemplates = _mapTemplateRepository.All()
                .Include(mt => mt.Maps)
                .Where(mt => mt.Maps!.Any(m => m.ProfileId == _profileService.GetProfileId()))
                .OrderByDescending(mt => mt.CreatedOn)
                .ToList();

            return _mapper.Map<IEnumerable<MapTemplateDto>>(mapTemplates);
        }

        public IEnumerable<MapTemplateDto> GetFeaturedMapTemplates()
        {
            if (!_memoryCache.TryGetValue(FeaturedMapTemplatesCacheKey, out IEnumerable<MapTemplateDto> cachedMapTemplates))
            {
                //TO BE CHANGED
                cachedMapTemplates = GetMapTemplates();

                var nextMonday = DateTime.Now.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek + 7) % 7).Date;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(nextMonday);

                _memoryCache.Set(FeaturedMapTemplatesCacheKey, cachedMapTemplates, cacheEntryOptions);
            }

            return cachedMapTemplates;
        }

        public async Task<string> GetMapTemplateJsonAsync(MapTemplateDto mapTemplate)
        {
            try
            {
                var templateFile = await _fileStorageManager.GetFileAsync(mapTemplate.FilePath);
                return Encoding.UTF8.GetString(templateFile!);
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException("Map template not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the map template.", ex);
            }
        }

        public async Task UploadMapTemplateAsync(MapTemplateDto mapTemplateDto, IFormFile file)
        {
            var mapTemplate = _mapper.Map<MapTemplate>(mapTemplateDto);

            mapTemplate.FilePath = await _fileStorageManager.SaveFileAsync(file, "MapTemplates", "File", mapTemplate.Id!);
            
            await _mapTemplateRepository.AddAsync(mapTemplate);
            await _mapTemplateRepository.SaveChangesAsync();
        }
    }
}
