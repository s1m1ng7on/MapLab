using MapLab.Data.Entities;
using MapLab.Services.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MapLab.Services.Contracts
{
    public interface IMapsService
    {
        public Task<MapDto> GetMapAsync(string id);
        public Task<(string, string)> GetMapJsonAsync(MapDto map);
        public IEnumerable<MapDto>? GetMapsForProfile(string profileId, bool isCurrentProfile);
        public Task CreateMapAsync(string name, string mapTemplateId, bool isPublic);
        public Task<(int likesCount, bool isLiked)> ToggleLikeDislikeMapAsync(string profileId, string mapId);
        public Task DeleteMapAsync(string id);
        public Task SaveMapAsync(string Id, string updatedMapJson);
        public Task UploadMapTemplateAsync(MapTemplate mapTemplate, IFormFile file);
        public Task<(MapDto, string, string)> OpenMapAsync(string id);
    }
}
