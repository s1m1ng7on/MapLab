using MapLab.Services.Models;

namespace MapLab.Services.Contracts
{
    public interface INewsService
    {
        Task<NewsPaginationDto> GetNewsAsync(int page, int pageSize);
        Task<NewsArticleDto> GetNewsArticleAsync(string id);
        Task CreateNewsArticleAsync(NewsArticleDto newsArticleDto);
        Task DeleteNewsArticleAsync(string id);
    }
}
