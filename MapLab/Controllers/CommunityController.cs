using Microsoft.AspNetCore.Mvc;

namespace MapLab.Controllers
{
    public class CommunityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
