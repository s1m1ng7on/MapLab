using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MapLab.Services
{
    public class MapsService : IMapsService
    {
        private readonly IDeletableEntityRepository<Map> _mapRepository;
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;
        private readonly IRepository<MapView> _mapViewsRepository;
        private readonly IDeletableEntityRepository<Like<Map>> _mapLikesRepository;
        private readonly IFileStorageManager _fileStorageManager;
        private readonly ITemplatesService _mapTemplatesService;
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;

        public MapsService(IDeletableEntityRepository<Map> mapRepository, IDeletableEntityRepository<MapTemplate> mapTemplateRepository, IRepository<MapView> mapViewsRepository, IDeletableEntityRepository<Like<Map>> mapLikesRepository, IFileStorageManager fileStorageManager, ITemplatesService mapTemplatesService, IProfileService profileService, IMapper mapper)
        {
            _mapRepository = mapRepository;
            _mapTemplateRepository = mapTemplateRepository;
            _mapViewsRepository = mapViewsRepository;
            _mapLikesRepository = mapLikesRepository;
            _fileStorageManager = fileStorageManager;
            _mapTemplatesService = mapTemplatesService;
            _profileService = profileService;
            _mapper = mapper;
        }

        public IEnumerable<MapDto>? GetMapsForProfile(string profileId, bool isCurrentProfile)
        {
            var maps = _mapRepository.All()
                    .Where(m => m.ProfileId == profileId && (isCurrentProfile || m.IsPublic))
                    .Include(m => m.Profile)
                    .Include(m => m.MapTemplate)
                    .Include(m => m.Views)
                    .Include(m => m.Likes)
                    .OrderByDescending(m => m.Views!
                        .OrderByDescending(mv => mv.CreatedOn)
                        .Select(mv => mv.CreatedOn)
                        .FirstOrDefault());

            return _mapper.Map<IEnumerable<MapDto>>(maps);
        }

        public async Task<MapDto?> GetMapAsync(string id)
        {
            var map = await _mapRepository.All()
                .Include(m => m.MapTemplate)
                .Include(m => m.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<MapDto>(map);
        }

        public async Task<(MapDto, string, string)> OpenMapAsync(string id)
        {
            var map = await GetMapAsync(id);

            var mapView = new MapView()
            {
                MapId = map.Id,
                ProfileId = _profileService.GetProfileId()
            };

            await _mapViewsRepository.AddAsync(mapView);
            await _mapViewsRepository.SaveChangesAsync();

            var (mapTemplateJson, mapJson) = await GetMapJsonAsync(map);

            return (map, mapTemplateJson, mapJson);
        }

        public async Task<(string, string)> GetMapJsonAsync(MapDto map)
        {
            var templateJson = await _mapTemplatesService.GetMapTemplateJsonAsync(map.MapTemplate!);

            string mapJson;
            try
            {
                var mapFile = await _fileStorageManager.GetFileAsync(map?.FilePath);
                mapJson = Encoding.UTF8.GetString(mapFile!);
            }
            catch
            {
                var emptyMapJsonObject = new JObject
                {
                    ["type"] = "FeatureCollection",
                    ["legend"] = new JArray(),
                    ["features"] = new JArray(),
                };

                mapJson = emptyMapJsonObject.ToString();
            }

            return (templateJson, mapJson);
        }

        public async Task CreateMapAsync(MapDto mapDto)
        {
            var map = _mapper.Map<Map>(mapDto);

            await _mapRepository.AddAsync(map);
            await _mapRepository.SaveChangesAsync();
        }

        public async Task DeleteMapAsync(string id)
        {
            var map = await _mapRepository.All()
                .Include(m => m.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (map == null)
                throw new InvalidOperationException("Map not found.");

            if (map.ProfileId != _profileService.GetProfileId())
                throw new UnauthorizedAccessException("You are not authorized to delete this map.");

            _mapRepository.Delete(map);
            await _mapRepository.SaveChangesAsync();
        }

        public async Task SaveMapAsync(string id, string updatedMapJson)
        {
            var map = await _mapRepository.FindAsync(id);
            map.FilePath = await _fileStorageManager.SaveJsonFileAsync(updatedMapJson, "Maps", "File", id);
            await _mapTemplateRepository.SaveChangesAsync();
        }

        public async Task UploadMapTemplateAsync(MapTemplate mapTemplate, IFormFile file)
        {
            mapTemplate.FilePath = await _fileStorageManager.SaveFileAsync(file, "MapTemplates", "File", mapTemplate.Id!);
            await _mapTemplateRepository.AddAsync(mapTemplate);
            await _mapTemplateRepository.SaveChangesAsync();
        }

        public async Task<(int likesCount, bool isLiked)> ToggleLikeDislikeMapAsync(string profileId, string mapId)
        {
            var existingLike = await _mapLikesRepository.All()
                .FirstOrDefaultAsync(ml => ml.ProfileId == profileId && ml.EntityId == mapId);

            if (existingLike != null)
            {
                _mapLikesRepository.Delete(existingLike);
            }
            else
            {
                var newLike = new Like<Map>
                {
                    EntityId = mapId,
                    ProfileId = profileId
                };

                await _mapLikesRepository.AddAsync(newLike);
            }

            await _mapLikesRepository.SaveChangesAsync();

            var likesCount = await _mapLikesRepository.All()
                .CountAsync(ml => ml.EntityId == mapId);

            var isLiked = await _mapLikesRepository.All()
                .AnyAsync(ml => ml.ProfileId == profileId && ml.EntityId == mapId);

            return (likesCount, isLiked);
        }
    }
}
