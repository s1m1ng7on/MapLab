using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Models.News
{
    public class NewsArticleViewModel : IMapFrom<NewsArticleDto>
    {
        public string? Title { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
