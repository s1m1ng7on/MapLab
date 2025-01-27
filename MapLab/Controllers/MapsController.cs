using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    public class MapsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
