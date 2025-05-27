using MapLab.Data.Entities;
using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Contracts
{
    public interface IMapsService
    {
        public Task<MapDto> GetMapAsync(string id);
        public Task<(string, string)> GetMapJsonAsync(MapDto map);
        public PaginationDto<MapDto> GetMaps(MapFiltersModel? filters = null, int page = 1, int pageSize = 10);
        public IEnumerable<MapDto>? GetMapsForProfile(string profileId, bool isCurrentProfile, MapFiltersModel? filters);
        public Task CreateMapAsync(MapDto mapDto);
        public Task<(int likesCount, bool isLiked)> ToggleLikeDislikeMapAsync(string profileId, string mapId);
        public Task EditMapAsync(MapDto mapDto);
        public Task DeleteMapAsync(string id);
        public Task SaveMapAsync(string Id, string updatedMapJson);
        public Task UploadMapTemplateAsync(MapTemplate mapTemplate, IFormFile file);
        public Task<(MapDto, string, string)> OpenMapAsync(string id);
    }
}
