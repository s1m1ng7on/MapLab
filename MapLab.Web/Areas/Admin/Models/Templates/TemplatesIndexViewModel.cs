using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;
using MapLab.Web.Models;

namespace MapLab.Web.Areas.Admin.Models.Templates
{
    public class TemplatesIndexViewModel : IMapFrom<PaginationDto<MapTemplateDto>>, IHasFilters<AdminTemplateFiltersModel>
    {
        public ICollection<TemplateViewModel>? Items { get; set; }
        public AdminTemplateFiltersModel? Filters { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
