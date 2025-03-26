using AutoMapper;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Web.Models;
using MapLab.Web.Models.Home;
using MapLab.Web.Models.News;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MapLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, INewsService newsService, IMapper mapper)
        {
            _logger = logger;
            _newsService = newsService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var newsArticles = await _newsService.GetNewsAsync(1, 3);

            var homeIndexViewModel = _mapper.Map<NewsPaginationDto, HomeIndexViewModel>(newsArticles);

            return View(homeIndexViewModel);
        }

        [Route("about")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult UserGuide()
        {
            return View();
        }

        [Route("faq")]
        public IActionResult FrequentlyAskedQuestions()
        {
            return View();
        }

        [Route("privacy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        [Route("terms")]
        public IActionResult TermsOfService()
        {
            return View();
        }

        [Route("NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
