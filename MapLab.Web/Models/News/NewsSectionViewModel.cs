using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Models.News
{
    public class NewsSectionViewModel : IMapFrom<NewsPaginationDto>
    {
        public List<NewsArticleCardViewModel>? Articles { get; set; }
    }
}
