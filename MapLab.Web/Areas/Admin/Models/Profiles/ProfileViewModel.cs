using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Areas.Admin.Models.Profiles
{
    public class ProfileViewModel : IMapFrom<ProfileDto>
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public string? PhoneNumber { get; set; }
        public int MapsCount { get; set; }
        public int MapTemplatesCount { get; set; }
    }
}
