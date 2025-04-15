using AutoMapper;
using MapLab.Data.Managers;
using MapLab.Services.Contracts;
using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;
using MapLab.Web.Models.Maps;
using MapLab.Web.Models.Templates;
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
        private readonly ITemplatesService _mapTemplatesService;
        private readonly IMapper _mapper;
        public MapsController(IProfileService profileService, ProfileManager<Profile> profileManager, IMapsService mapsService, ITemplatesService mapTemplatesService, IMapper mapper)
        {
            _profileService = profileService;
            _profileManager = profileManager;
            _mapsService = mapsService;
            _mapTemplatesService = mapTemplatesService;
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
                Maps = maps?.Select(map =>
                {
                    var mapCardViewModel = _mapper.Map<MapDto, MapViewModel>(map);
                    mapCardViewModel.IsByCurrentProfile = isCurrentProfile;
                    return mapCardViewModel;
                }),
                ProfileUserName = profileUserName,
                IsCurrentProfile = isCurrentProfile,
                MapCreateViewModel = isCurrentProfile
                    ? new MapCreateViewModel()
                    {
                        RecentMapTemplates = _mapper.Map<List<MapTemplateViewModel>>(_mapTemplatesService.GetRecentMapTemplates()),
                        MapLabMapTemplates = _mapper.Map<List<MapTemplateViewModel>>(_mapTemplatesService.GetMapTemplates(new MapTemplateFiltersModel() { ByMapLab = true })),
                        FeaturedMapTemplates = _mapper.Map<List<MapTemplateViewModel>>(_mapTemplatesService.GetFeaturedMapTemplates())
                    }
                    : null
            };

            return View(mapsIndexViewModel);
        }

        [Route("map/{id}")]
        public async Task<IActionResult> View(string id)
        {
            var map = await _mapsService.OpenMapAsync(id);
            var mapViewModel = _mapper.Map<MapComponentViewModel>(map.Item1);
            mapViewModel.CanEdit = map.Item1.ProfileId == _profileService.GetProfileId();
            (mapViewModel.MapTemplateJson, mapViewModel.MapJson) = (map.Item2, map.Item3);

            return View("BlazorView", mapViewModel);
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
        [Route("[controller]/[action]")]
        [Authorize]
        public async Task<IActionResult> Create(MapCreateViewModel mapCreateViewModel)
        {
            var mapDto = _mapper.Map<MapDto>(mapCreateViewModel);
            await _mapsService.CreateMapAsync(mapDto);

            return RedirectToAction("Index");
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
        [ValidateAntiForgeryToken]
        [Route("map/[action]/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await _mapsService.DeleteMapAsync(id);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
