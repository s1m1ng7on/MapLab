using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Web.Models.Maps;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    [Route("[controller]")]
    public class MapsController : Controller
    {
        private readonly IMapService _mapService;
        private readonly IMapper _mapper;

        public MapsController(IMapService mapService, IMapper mapper)
        {
            _mapService = mapService;
            _mapper = mapper;
        }

        [Route("{profileUserName?}")]
        public async Task<IActionResult> Index(string profileUserName)
        {
            var maps = await _mapService.GetMapsForProfile(profileUserName);
            var mapViewModels = maps?.Select(_mapper.Map<Map, MapViewModel>).ToList();

            return View(mapViewModels);
        }
    }
}
