using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;
using MapLab.Shared.Models.FilterModels;
using MapLab.Web.Areas.Admin.Models;
using MapLab.Web.Areas.Admin.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TemplatesController : Controller
    {
        private readonly ITemplatesService _mapTemplatesService;
        private readonly IMapsService _mapService;
        private readonly IMapper _mapper;

        public TemplatesController(ITemplatesService mapTemplatesService, IMapsService mapService, IMapper mapper)
        {
            _mapTemplatesService = mapTemplatesService;
            _mapService = mapService;
            _mapper = mapper;
        }

        public IActionResult Index(AdminTemplateFiltersModel filters, int page = 1)
        {
            var mapTemplates = _mapper.Map<TemplatesIndexViewModel>(_mapTemplatesService.GetMapTemplates(filters));
            return View(mapTemplates);
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
