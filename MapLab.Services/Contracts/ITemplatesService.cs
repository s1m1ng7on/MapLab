using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;

namespace MapLab.Services.Contracts
{
    public interface ITemplatesService
    {
        public Task<MapTemplateDto> GetMapTemplateAsync(string id);
        public IEnumerable<MapTemplateDto> GetMapTemplates(MapTemplateFiltersModel? filters = null);
        public IEnumerable<MapTemplateDto> GetRecentMapTemplates();
        public IEnumerable<MapTemplateDto> GetFeaturedMapTemplates();
    }
}
