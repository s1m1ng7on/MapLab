using AutoMapper;
using MapLab.Data;
using MapLab.Data.Managers;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Web.Models.Maps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile = MapLab.Data.Entities.Profile;

namespace MapLab.Web.Controllers
{
    public class MapsController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ProfileManager<Profile> _profileManager;
        private readonly IMapsService _mapsService;
        private readonly IMapper _mapper;
        public MapsController(IProfileService profileService, ProfileManager<Profile> profileManager, IMapsService mapsService, IMapper mapper)
        {
            _profileService = profileService;
            _profileManager = profileManager;
            _mapsService = mapsService;
            _mapper = mapper;
        }

        [Route("[controller]/{profileUserName?}")]
        public async Task<IActionResult> Index(string profileUserName)
        {
            if (string.IsNullOrEmpty(profileUserName) && !User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Index", "Maps") });
            }

            var (profileId, isCurrentProfile) = string.IsNullOrEmpty(profileUserName)
                ? (_profileService.GetProfileId(), true)
                : (await _profileManager.FindByNameAsync(profileUserName) is var profile
                    ? (profile?.Id, profile?.Id == _profileService.GetProfileId())
                    : throw new Exception("Profile not found"));

            var maps = _mapsService.GetMapsForProfile(profileId!, isCurrentProfile);

            var mapsIndexViewModel = new MapsIndexViewModel()
            {
                Maps = maps?.Select(_mapper.Map<MapDto, MapViewModel>).ToList(),
                ProfileUserName = profileUserName,
                IsCurrentProfile = isCurrentProfile
            };

            return View(mapsIndexViewModel);
        }

        [Route("map/{id}")]
        public IActionResult View(string id)
        {
            return View("BlazorView", id);
        }

        [Route("map/[action]/{id}")]
        public async Task<IActionResult> Info(string id)
        {
            var map = await _mapsService.GetMapAsync(id);
            var mapViewModel = _mapper.Map<MapViewModel>(map);

            return View(mapViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("map/[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> Like([FromRoute] string id)
        {
            var profileId = _profileService.GetProfileId();

            var (likesCount, isLiked) = await _mapsService.ToggleLikeDislikeMapAsync(profileId!, id);

            return Json(new
            {
                success = true,
                likesCount,
                isLiked
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMap(string id)
        {
            await _mapsService.DeleteMapAsync(id);
            return RedirectToAction("Index");
        }
    }
}
