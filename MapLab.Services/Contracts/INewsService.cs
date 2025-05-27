using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;

namespace MapLab.Services.Contracts
{
    public interface INewsService
    {
        Task<PaginationDto<NewsArticleDto>> GetNewsAsync(int page, int pageSize, NewsFilterModel? filters = null);
        Task<NewsArticleDto> GetNewsArticleAsync(string id);
        Task CreateNewsArticleAsync(NewsArticleDto newsArticleDto);
        Task EditNewsArticleAsync(NewsArticleDto newsArticleDto, string oldContent);
        Task DeleteNewsArticleAsync(string id);
    }
}
