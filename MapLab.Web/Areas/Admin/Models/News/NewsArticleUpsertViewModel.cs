using MapLab.Common.Enums;
using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Areas.Admin.Models.News
{
    public class NewsArticleUpsertViewModel : IMapFrom<NewsArticleDto>, IMapTo<NewsArticleDto>
    {
        public CrudOperation CrudOperation { get; set; }
        public string? Id { get; set; }
        public string? Title { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? Content { get; set; }
    }
}
