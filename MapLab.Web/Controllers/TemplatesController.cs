using AutoMapper;
using MapLab.Services.Contracts;
using MapLab.Web.Areas.Admin.Models;
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
            var mapTemplateViewModel = _mapper.Map<MapTemplateViewModel>(mapTemplate);

            return View(mapTemplateViewModel);
        }
    }
}
