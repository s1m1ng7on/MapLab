using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;

namespace MapLab.Services.Contracts
{
    public interface IProfilesService
    {
        PaginationDto<ProfileDto> GetProfiles(ProfileFiltersModel? filters = null, int page = 1, int pageSize = 10);
    }
}
