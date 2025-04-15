using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Contracts
{
    public interface ITemplatesService
    {
        public Task<MapTemplateDto> GetMapTemplateAsync(string id);
        public IEnumerable<MapTemplateDto> GetMapTemplates(MapTemplateFiltersModel? filters = null);
        public IEnumerable<MapTemplateDto> GetRecentMapTemplates();
        public IEnumerable<MapTemplateDto> GetFeaturedMapTemplates();
        public Task<string> GetMapTemplateJsonAsync(MapTemplateDto mapTemplate);
        public Task UploadMapTemplateAsync(MapTemplateDto mapTemplateDto, IFormFile file);
    }
}
