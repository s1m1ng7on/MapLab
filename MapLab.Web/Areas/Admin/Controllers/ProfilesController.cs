using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Services.Contracts;
using MapLab.Shared.Areas.Admin.Models;
using MapLab.Web.Areas.Admin.Models.Profiles;
using MapLab.Web.Areas.Admin.Models.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MapLab.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProfilesController : Controller
    {
        private readonly IProfilesService _profilesService;
        private readonly IMapper _mapper;

        public ProfilesController(IProfilesService profilesService, IMapper mapper)
        {
            _profilesService = profilesService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(ProfileFiltersModel filters, int page = 1)
        {
            var profiles = _mapper.Map<IndexViewModel>(_profilesService.GetProfiles(filters, page));
            profiles.Filters = filters;

            return View(profiles);
        }
    }
}
