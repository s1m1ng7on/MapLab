using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Contracts
{
    public interface ITemplatesService
    {
        public Task<MapTemplateDto> GetMapTemplateAsync(string id);
        public PaginationDto<MapTemplateDto> GetMapTemplates(MapTemplateFiltersModel? filters = null, int page = 1, int pageSize = 10);
        public PaginationDto<MapTemplateDto> GetRecentMapTemplates(int page = 1, int pageSize = 10);
        public Task<PaginationDto<MapTemplateDto>> GetFeaturedMapTemplates(int page = 1, int pageSize = 10);
        public Task<string> GetMapTemplateJsonAsync(MapTemplateDto mapTemplate);
        public Task UploadMapTemplateAsync(MapTemplateDto mapTemplateDto, IFormFile file);
    }
}
