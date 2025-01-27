using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    [Route("[action]")]
    public class LegalController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }
    }
}
