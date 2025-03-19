namespace MapLab.Services.Models
{
    public class NewsPaginationDto
    {
        public List<NewsArticleDto>? Articles { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
