using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;
using MapLab.Shared.Models.FilterModels;
using MapLab.Web.Models;
using MapLab.Web.Models.Maps;

namespace MapLab.Web.Areas.Admin.Models.Maps
{
    public class IndexViewModel : IMapFrom<PaginationDto<MapDto>>, IHasFilters<AdminMapFiltersModel>
    {
        public ICollection<MapViewModel>? Items { get; set; }
        public AdminMapFiltersModel? Filters { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
