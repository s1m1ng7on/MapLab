using AutoMapper;
using MapLab.Data.Managers;
using MapLab.Web.Models.Profile;
using Microsoft.AspNetCore.Mvc;
using ProfileEntity = MapLab.Data.Entities.Profile;

namespace MapLab.Web.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ProfileManager<ProfileEntity> _profileManager;
        private readonly IMapper _mapper;

        public ProfileController(ProfileManager<ProfileEntity> profileManager, IMapper mapper)
        {
            _profileManager = profileManager;
            _mapper = mapper;
        }

        [Route("{username?}")]
        public async Task<IActionResult> Index(string username)
        {
            var profile = !string.IsNullOrEmpty(username)
                ? (await _profileManager.FindByNameAsync(username))
                : User.Identity?.IsAuthenticated == true
                    ? (await _profileManager.GetUserAsync(User))
                    : null;

            if (profile == null)
                return NotFound();

            var profileViewModel = _mapper.Map<ProfileViewModel>(profile);

            var currentProfile = await _profileManager.GetUserAsync(User);
            profileViewModel.IsCurrentProfile = currentProfile.Id == profile.Id;
            profileViewModel.IsAdmin = await _profileManager.IsInRoleAsync(profile, "Admin");

            return View(profileViewModel);
        }
    }
}
