using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Services.Contracts;
using MapLab.Web.Models.Maps;
using Microsoft.AspNetCore.Mvc;
using Profile = MapLab.Data.Entities.Profile;

namespace MapLab.Web.Controllers
{
    public class MapsController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ProfileManager<Profile> _profileManager;
        private readonly IMapsService _mapService;
        private readonly IMapper _mapper;

        public MapsController(IProfileService profileService, ProfileManager<Profile> profileManager, IMapsService mapService, IMapper mapper)
        {
            _profileService = profileService;
            _profileManager = profileManager;
            _mapService = mapService;
            _mapper = mapper;
        }

        [Route("[controller]/{profileUserName?}")]
        public async Task<IActionResult> Index(string profileUserName)
        {
            var (profileId, isCurrentProfile) = string.IsNullOrEmpty(profileUserName)
                ? (_profileService.GetProfileId(), true)
                : (await _profileManager.FindByNameAsync(profileUserName) is var profile
                    ? (profile?.Id, profile?.Id == _profileService.GetProfileId())
                    : throw new Exception("Profile not found"));

            var maps = _mapService.GetMapsForProfile(profileId!, isCurrentProfile);

            var mapsIndexViewModel = new MapsIndexViewModel()
            {
                Maps = maps?.Select(_mapper.Map<Map, MapViewModel>).ToList(),
                ProfileUserName = profileUserName,
                IsCurrentProfile = isCurrentProfile
            };

            return View(mapsIndexViewModel);
        }

        [Route("map/{id}")]
        public async Task<IActionResult> View(string id)
        {
            return View("BlazorView", id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMap(string id)
        {
            await _mapService.DeleteMapAsync(id);
            return RedirectToAction("Index");
        }
    }
}
