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

            var maps = _mapService.GetMapsForProfile(profileId!);

            var mapsIndexViewModel = new MapsIndexViewModel()
            {
                Maps = maps?.Select(_mapper.Map<Map, MapViewModel>).ToList(),
                ProfileUserName = profileUserName,
                IsCurrentProfile = isCurrentProfile
            };

            return View(mapsIndexViewModel);
        }

        [Route("map/{id}")]
        public IActionResult View(string id)
        {
            Response.Cookies.Append("mapIdCookie", id, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(1),
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return View();
        }

        [Route("api/map/{id}")]
        public async Task<IActionResult> GetMapApi(string id)
        {
            var map = await _mapService.GetMapAsync(id);
            return Json(map);
        }
    }
}
