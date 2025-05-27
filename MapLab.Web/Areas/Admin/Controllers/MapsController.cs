using AutoMapper;
using MapLab.Services.Contracts;
using MapLab.Shared.Areas.Admin.Models;
using MapLab.Shared.Models.FilterModels;
using MapLab.Web.Areas.Admin.Models.Maps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MapsController : Controller
    {
        private readonly IMapsService _mapsService;
        private readonly IMapper _mapper;

        public MapsController(IMapsService mapsService, IMapper mapper)
        {
            _mapsService = mapsService;
            _mapper = mapper;
        }

        public IActionResult Index(AdminMapFiltersModel? filters = null, int page = 1)
        {
            var maps = _mapper.Map<IndexViewModel>(_mapsService.GetMaps(null, page, 10));
            maps.Filters = filters;

            return View(maps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[area]/map/[action]")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _mapsService.DeleteMapAsync(id);
                return RedirectToAction("Index", new { area = "Admin" });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
