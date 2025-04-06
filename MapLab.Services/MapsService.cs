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
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Mail;
using System.Text;

namespace MapLab.Services
{
    public class MapsService : IMapsService
    {
        private readonly IDeletableEntityRepository<Map> _mapRepository;
        private readonly IDeletableEntityRepository<MapTemplate> _mapTemplateRepository;
        private readonly IDeletableEntityRepository<Like<Map>> _mapLikesRepository;
        private readonly IFileStorageManager _fileStorageManager;
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;

        public MapsService(IDeletableEntityRepository<Map> mapRepository, IDeletableEntityRepository<MapTemplate> mapTemplateRepository, IDeletableEntityRepository<Like<Map>> mapLikesRepository, IFileStorageManager fileStorageManager, IProfileService profileService, IMapper mapper)
        {
            _mapRepository = mapRepository;
            _mapTemplateRepository = mapTemplateRepository;
            _mapLikesRepository = mapLikesRepository;
            _fileStorageManager = fileStorageManager;
            _profileService = profileService;
            _mapper = mapper;
        }

        public IEnumerable<MapDto>? GetMapsForProfile(string profileId, bool isCurrentProfile)
        {
            var maps = _mapRepository.All()
                    .Where(m => m.ProfileId == profileId && (isCurrentProfile || m.IsPublic))
                    .Include(m => m.Profile)
                    .Include(m => m.Template)
                    .Include(m => m.Views)
                    .Include(m => m.Likes);

            return _mapper.Map<IEnumerable<MapDto>>(maps);
        }

        public async Task<MapDto?> GetMapAsync(string id)
        {
            var map = await _mapRepository.All()
                .Include(m => m.Template)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<MapDto>(map);
        }

        public async Task<(string, JObject)> GetMapJsonAsync(MapDto map)
        {
            var templateFile = await _fileStorageManager.GetFileAsync(map?.Template?.FilePath);
            var mapFile = await _fileStorageManager.GetFileAsync(map?.FilePath);

            string templateJson = Encoding.UTF8.GetString(templateFile!);

            JObject mapJsonObject;

            try
            {
                string mapJson = Encoding.UTF8.GetString(mapFile!);
                mapJsonObject = JObject.Parse(mapJson);
            }
            catch
            {
                mapJsonObject = new JObject
                {
                    ["type"] = "FeatureCollection",
                    ["legend"] = new JArray(),
                    ["features"] = new JArray(),
                };
            }

            return (templateJson, mapJsonObject);
        }

        public async Task CreateMapAsync(string name, string mapTemplateId, bool isPublic)
        {
            Map newMap = new Map()
            {
                Name = name,
                TemplateId = mapTemplateId,
                IsPublic= isPublic,
                ProfileId = _profileService.GetProfileId()
            };

            await _mapRepository.AddAsync(newMap);
            await _mapRepository.SaveChangesAsync();
        }

        public async Task DeleteMapAsync(string id)
        {
            var map = await _mapRepository.FindAsync(id);
            _mapRepository.Delete(map!);
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
