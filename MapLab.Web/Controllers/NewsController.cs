using AutoMapper;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Web.Models.News;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    [Route("[controller]")]
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
            var newsArticles = await _newsService.GetNewsAsync(1, 7 * 3);
            var newsIndexViewModel = _mapper.Map<NewsPaginationDto, NewsSectionViewModel>(newsArticles);

            return View(newsIndexViewModel);
        }

        [Route("[action]/{id}")]
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
