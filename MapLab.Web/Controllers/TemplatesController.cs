using AutoMapper;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Web.Models.Maps;
using MapLab.Web.Models.Templates;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly ITemplatesService _mapTemplatesService;
        private readonly IMapper _mapper;

        public TemplatesController(ITemplatesService mapTemplatesService, IMapper mapper)
        {
            _mapTemplatesService = mapTemplatesService;
            _mapper = mapper;
        }

        [Route("template/{id}")]
        public async Task<IActionResult> View(string id)
        {
            var mapTemplate = await _mapTemplatesService.GetMapTemplateAsync(id);
            var mapTemplateComponentViewModel = _mapper.Map<MapTemplateComponentViewModel>(mapTemplate);
            mapTemplateComponentViewModel.MapTemplateJson = await _mapTemplatesService.GetMapTemplateJsonAsync(mapTemplate);

            return View(mapTemplateComponentViewModel);
        }

        [Route("template/[action]/{id}")]
        public async Task<IActionResult> Info(string id)
        {
            var mapTemplate = await _mapTemplatesService.GetMapTemplateAsync(id);
            var mapTemplateViewModel = _mapper.Map<MapTemplateViewModel>(mapTemplate);

            return View(mapTemplateViewModel);
        }
    }
}
