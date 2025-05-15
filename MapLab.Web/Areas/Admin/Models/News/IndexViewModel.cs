using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;

namespace MapLab.Web.Areas.Admin.Models.News
{
    public class IndexViewModel : IMapFrom<PaginationDto<NewsArticleDto>>, IHasFilters<NewsFilterModel>
    {
        public List<NewsArticleTableRowViewModel>? Items { get; set; }
        public NewsFilterModel? Filters { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
