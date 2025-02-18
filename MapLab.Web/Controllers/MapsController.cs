using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Services.Contracts;
using MapLab.Web.Models.Maps;
using Microsoft.AspNetCore.Mvc;
using Profile = MapLab.Data.Entities.Profile;

namespace MapLab.Web.Controllers
{
    [Route("[controller]")]
    public class MapsController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ProfileManager<Profile> _profileManager;
        private readonly IMapService _mapService;
        private readonly IMapper _mapper;

        public MapsController(IProfileService profileService, ProfileManager<Profile> profileManager, IMapService mapService, IMapper mapper)
        {
            _profileService = profileService;
            _profileManager = profileManager;
            _mapService = mapService;
            _mapper = mapper;
        }

        [Route("{profileUserName?}")]
        public async Task<IActionResult> Index(string profileUserName)
        {
            var (profileId, isCurrentProfile) = string.IsNullOrEmpty(profileUserName)
                ? (_profileService.GetProfileId(), true)
                : ((await _profileManager.FindByNameAsync(profileUserName)).Id, false);

            var maps = await _mapService.GetMapsForProfile(profileId);

            var mapsIndexViewModel = new MapsIndexViewModel()
            {
                Maps = maps?.Select(_mapper.Map<Map, MapViewModel>).ToList(),
                ProfileUserName = profileUserName,
                IsCurrentProfile = isCurrentProfile
            };

            return View(mapsIndexViewModel);
        }
    }
}
