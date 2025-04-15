using AutoMapper;
using MapLab.Common.Enums;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Web.Areas.Admin.Models.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize]
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
            var newsArticle = new NewsArticleUpsertViewModel()
            {
                CrudOperation = CrudOperation.Create
            };

            return View("Upsert", newsArticle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsArticleUpsertViewModel newsArticle)
        {
            var newsArticleDto = _mapper.Map<NewsArticleDto>(newsArticle);
            await _newsService.CreateNewsArticleAsync(newsArticleDto);

            return RedirectToAction("Index", new { area = "Admin" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var newsArticle = await _newsService.GetNewsArticleAsync(id);
            var newsArticleViewModel = _mapper.Map<NewsArticleUpsertViewModel>(newsArticle);

            TempData["EditingArticleId"] = newsArticle.Id;
            TempData["NewsArticleOldContent"] = newsArticle.Content;

            newsArticleViewModel.CrudOperation = CrudOperation.Update;

            return View("Upsert", newsArticleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsArticleUpsertViewModel newsArticle)
        {
            if (!ModelState.IsValid)
                return View(newsArticle);

            var newsArticleDto = _mapper.Map<NewsArticleDto>(newsArticle);

            var oldContent = TempData["NewsArticleOldContent"] as string;

            await _newsService.EditNewsArticleAsync(newsArticleDto, oldContent);

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
