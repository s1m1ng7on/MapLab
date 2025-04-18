using AutoMapper;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Web.Models;
using MapLab.Web.Models.Home;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MapLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly IMapper _mapper;
        private readonly INotifierService _notifierService;

        public HomeController(ILogger<HomeController> logger, INewsService newsService, IMapper mapper, INotifierService notifierService)
        {
            _logger = logger;
            _newsService = newsService;
            _mapper = mapper;
            _notifierService = notifierService;
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

        [Route("notfound")]
        public IActionResult NotFound()
        {
            return View();
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            var statusCode = HttpContext.Response.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
                return base.NotFound();

            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode
            };

            _logger.LogError($"Error occurred with Status Code: {statusCode}, RequestId: {errorViewModel.RequestId}");

            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception != null)
            {
                _logger.LogError(exception, exception.Message);
                await _notifierService.NotifyAdminsAboutError(exception, HttpContext);
            }

            return View(errorViewModel);
        }
    }
}
