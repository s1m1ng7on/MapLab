using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    [Route("[controller]")]
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("{id}")]
        public IActionResult Index(string id)
        {
            return View();
        }
    }
}
