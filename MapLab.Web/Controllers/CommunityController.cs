using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Services.Contracts;
using MapLab.Web.Models.Community;
using MapLab.Web.Models.Maps;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Controllers
{
    public class CommunityController : Controller
    {
        private readonly IMapAnalyticsService _mapAnalyticsService;
        private readonly IMapper _mapper;

        public CommunityController(IMapAnalyticsService mapAnalyticsService, IMapper mapper)
        {
            _mapAnalyticsService = mapAnalyticsService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var topMaps = _mapAnalyticsService.GetTopMapsForWeek().Select(_mapper.Map<Map, MapViewModel>);

            var model = new IndexViewModel
            {
                TopMaps = topMaps
            };

            return View(model);
        }
    }
}
