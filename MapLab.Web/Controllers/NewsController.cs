using AutoMapper;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Web.Models.News;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public NewsController(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var newsArticles = await _newsService.GetNewsAsync(1, 5 * 3);
            var newsIndexViewModel = _mapper.Map<PaginationDto<NewsArticleDto>, NewsSectionViewModel>(newsArticles);

            return View(newsIndexViewModel);
        }

        public async Task<IActionResult> Load(int page = 1)
        {
            var newsArticles = await _newsService.GetNewsAsync(page, 5 * 3);
            var newsArticlesViewModel = _mapper.Map<PaginationDto<NewsArticleDto>, NewsSectionViewModel>(newsArticles);

            return PartialView("_NewsArticleCardsPartial", newsArticlesViewModel);
        }

        public async Task<IActionResult> Article(string id)
            {
            var newsArticle = await _newsService.GetNewsArticleAsync(id);

            if (newsArticle == null)
                return NotFound();

            var newsArticleViewModel = _mapper.Map<NewsArticleDto, NewsArticleViewModel>(newsArticle);

            return View(newsArticleViewModel);
        }
    }
}
