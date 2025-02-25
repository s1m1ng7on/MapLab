using MapLab.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MapLab.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetProfileId()
            => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
