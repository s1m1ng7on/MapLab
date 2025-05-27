using AutoMapper;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using MapLab.Services.Extensions;
using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Profile = MapLab.Data.Entities.Profile;

namespace MapLab.Services
{
    public class ProfilesService : IProfilesService
    {
        private readonly IRepository<ProfileDto> _profileRepository;
        private readonly IMapper _mapper;

        public ProfilesService(IRepository<ProfileDto> profilesService, IMapper mapper)
        {
            _profileRepository = profilesService;
            _mapper = mapper;
        }

        public PaginationDto<ProfileDto> GetProfiles(ProfileFiltersModel? filters = null, int page = 1, int pageSize = 10)
        {
            var profiles = _profileRepository.All()
                .Include(p => p.Maps)
                .Include(p => p.MapTemplates)
                .Include(p => p.MapViews)
                .AsQueryable();

            if (filters != null)
            {
                //Implement
            }

            //return profiles.ToPaginationDto<Profile, ProfileDto>(_mapper, page, pageSize);
            return null;
        }
    }
}
