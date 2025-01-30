using MapLab.Data.Entities;
using MapLab.Data.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MapLab.Web.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ProfileManager<Profile> _profileManager;

        public ProfileController(ProfileManager<Profile> profileManager)
        {
            _profileManager = profileManager;
        }

        [Route("{username?}")]
        public async Task<IActionResult> Index(string username)
        {
            var user = !string.IsNullOrEmpty(username)
                ? (await _profileManager.FindByNameAsync(username))
                : User.Identity?.IsAuthenticated == true
                    ? (await _profileManager.GetUserAsync(User))
                    : null;

            if (user == null)
                return NotFound();

            return View(user);
        }
    }
}
