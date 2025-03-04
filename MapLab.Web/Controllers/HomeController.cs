using MapLab.Services.Contracts;
using MapLab.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MapLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapsService _mapService;

        public HomeController(ILogger<HomeController> logger, IMapsService mapService)
        {
            _logger = logger;
            _mapService = mapService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult AboutUs()
        {
            return View();
        }

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
