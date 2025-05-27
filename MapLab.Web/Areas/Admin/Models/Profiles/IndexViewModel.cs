using MapLab.Data.Entities;
using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;
using MapLab.Web.Areas.Admin.Models.Templates;
using MapLab.Web.Models;

namespace MapLab.Web.Areas.Admin.Models.Profiles
{
    public class IndexViewModel : IMapFrom<PaginationDto<ProfileDto>>, IHasFilters<ProfileFiltersModel>
    {
        public ICollection<ProfileViewModel>? Items { get; set; }
        public ProfileFiltersModel? Filters { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
