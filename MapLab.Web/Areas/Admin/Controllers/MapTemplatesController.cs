using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Services.Contracts;
using MapLab.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MapTemplatesController : Controller
    {
        private readonly IMapService _mapService;
        private readonly IMapper _mapper;

        public MapTemplatesController(IMapService mapService, IMapper mapper)
        {
            _mapService = mapService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new MapTemplateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(MapTemplateViewModel mapTemplateViewModel)
        {
            if (ModelState.IsValid)
            {
                var mapTemplate = _mapper.Map<MapTemplate>(mapTemplateViewModel);

                await _mapService.UploadMapTemplateAsync(mapTemplate, mapTemplateViewModel.File);

                return RedirectToAction("Index");
            }

            return View(mapTemplateViewModel);
        }
    }
}
