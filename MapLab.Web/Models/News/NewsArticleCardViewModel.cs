using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Models.News
{
    public class NewsArticleCardViewModel : IMapFrom<NewsArticleDto>
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
