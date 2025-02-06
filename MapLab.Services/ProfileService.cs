using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MapLab.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ProfileManager<Profile> _profileManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(ProfileManager<Profile> profileManager, IHttpContextAccessor httpContextAccessor)
        {
            _profileManager = profileManager;

            _httpContextAccessor = httpContextAccessor;
        }

        public string GetProfileId()
            => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
