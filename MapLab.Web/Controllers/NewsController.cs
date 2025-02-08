using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
