using MapLab.Data.Entities;
using MapLab.Services.Mapping;
using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Models
{
    public class NewsArticleDto : IMapFrom<NewsArticle>, IMapTo<NewsArticle>
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public string? Content { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
