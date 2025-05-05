using AutoMapper;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;
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

        public async Task<IActionResult> Load(string type, int page = 1, int pageSize = 10)
        {
            PaginationDto<MapTemplateDto> templates;

            switch (type.ToLower())
            {
                case "recent":
                    templates = _mapTemplatesService.GetRecentMapTemplates(page, pageSize);
                    break;
                case "by-maplab":
                    templates = _mapTemplatesService.GetMapTemplates(new MapTemplateFiltersModel { ByMapLab = true }, page, pageSize);
                    break;
                case "featured":
                    templates = await _mapTemplatesService.GetFeaturedMapTemplates(page, pageSize);
                    break;
                default:
                    return BadRequest("Invalid template type.");
            }

            var viewModels = _mapper.Map<List<MapTemplateViewModel>>(templates.Items);
            return PartialView("_MapTemplateCardListPartial", viewModels);
        }

        public async Task<IActionResult> Search(MapTemplateFiltersModel filters)
        {
            var mapTemplates = _mapTemplatesService.GetMapTemplates(filters);
            var viewModels = _mapper.Map<List<MapTemplateViewModel>>(mapTemplates.Items);
            return PartialView("_MapTemplatesSearchResultsPartial", viewModels);
        }
    }
}
