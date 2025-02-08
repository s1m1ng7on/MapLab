using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Services.Contracts;
using MapLab.Web.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    public class TemplatesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMapService _mapService;

        public TemplatesController(IMapper mapper, IMapService mapService)
        {
            _mapper = mapper;
            _mapService = mapService;
        }

        public IActionResult Index(string name)
        {
            var templates = _mapService.GetMapTemplates(name);

            return Json(templates);
        }

        public IActionResult Upload()
        {
            var uploadViewModel = new UploadViewModel();
            return View(uploadViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadViewModel uploadViewModel)
        {
            if (!ModelState.IsValid)
                return View(uploadViewModel);

            var template = _mapper.Map<MapTemplate>(uploadViewModel);
            await _mapService.UploadMapTemplateAsync(template);
            return Redirect("/");
        }
    }
}
