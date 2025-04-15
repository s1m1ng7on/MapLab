using AutoMapper;
using MapLab.Data.Entities;
using MapLab.Services.Contracts;
using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Data.Models.Enums;

namespace MapLab.Web.Models.Maps
{
    public class MapViewModel : IMapFrom<MapDto>, IHasCustomMappings
    { 
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public string? MapTemplateId { get; set; }
        public string? MapTemplateName { get; set; }
        public Region? MapTemplateRegion { get; set; }
        public bool IsPublic { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public bool IsByCurrentProfile { get; set; } = false;
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public bool IsLikedByCurrentProfile { get; set; }

        public void CreateMappings(IProfileExpression configuration, IServiceProvider serviceProvider)
        {
            var profileService = serviceProvider.GetService<IProfileService>();

            if (profileService != null)
            {
                configuration.CreateMap<MapDto, MapViewModel>()
                    .ForMember(m => m.IsLikedByCurrentProfile,
                        opt => opt.MapFrom((map, vm, destMember, context) =>
                        {
                            var currentProfileId = profileService.GetProfileId();
                            return map.Likes?.Any(like => currentProfileId == like.ProfileId);
                        }));
            }
        }
    }
}
