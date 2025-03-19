using MapLab.Data.Entities;
using MapLab.Services.Mapping;

namespace MapLab.Services.Models
{
    public class NewsArticleDto : IMapFrom<NewsArticle>, IMapTo<NewsArticle>
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
