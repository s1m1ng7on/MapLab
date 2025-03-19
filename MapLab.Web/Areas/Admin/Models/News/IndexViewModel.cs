using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Areas.Admin.Models.News
{
    public class IndexViewModel : IMapFrom<NewsPaginationDto>
    {
        public List<NewsArticleTableRowViewModel>? Articles { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; }
    }
}
