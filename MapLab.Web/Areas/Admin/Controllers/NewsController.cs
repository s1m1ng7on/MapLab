using AutoMapper;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Web.Areas.Admin.Models.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public NewsController(INewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var newsArticles = _mapper.Map<NewsPaginationDto, IndexViewModel>(await _newsService.GetNewsAsync(page, pageSize));
            return View(newsArticles);
        }

        public IActionResult Create()
        {
            var newsArticle = new NewsArticleEditViewModel();
            return View(newsArticle);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsArticleEditViewModel newsArticle)
        {
            var newsArticleDto = _mapper.Map<NewsArticleEditViewModel, NewsArticleDto>(newsArticle);
            await _newsService.CreateNewsArticleAsync(newsArticleDto);

            return RedirectToAction("Index", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _newsService.DeleteNewsArticleAsync(id);
            return RedirectToAction("Index", new { area = "Admin" });
        }
    }
}
