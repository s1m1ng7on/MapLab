using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Areas.Admin.Models.News
{
    public class NewsArticleEditViewModel : IMapTo<NewsArticleDto>
    {
        public string? Title { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? Content { get; set; }
    }
}
