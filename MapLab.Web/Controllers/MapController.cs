using MapLab.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    public class MapController : Controller
    {
        private readonly IMapService _mapService;

        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }

        [Route("[controller]/{id}")]
        public IActionResult Index(string id)
        {
            Response.Cookies.Append("mapIdCookie", id, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(1),
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return View();
        }

        [Route("api/[controller]/{id}")]
        public async Task<IActionResult> IndexApi(string id)
        {
            var map = await _mapService.GetMapAsync(id);
            return Json(map);
        }
    }
}
