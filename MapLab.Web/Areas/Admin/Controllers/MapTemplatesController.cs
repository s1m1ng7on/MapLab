using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MapTemplatesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
