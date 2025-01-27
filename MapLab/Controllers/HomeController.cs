using MapLab.Services.Contracts;
using MapLab.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MapLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapService _mapService;

        public HomeController(ILogger<HomeController> logger, IMapService mapService)
        {
            _logger = logger;
            _mapService = mapService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult MapTest(string id = "0ed69a24-d677-4ecf-8602-fbb1d8c2cc67")
        {
            return View((object)id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
