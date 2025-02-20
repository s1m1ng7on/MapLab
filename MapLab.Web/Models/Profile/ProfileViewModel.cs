using MapLab.Data.Entities;
using MapLab.Services.Mapping;
using ProfileEntity = MapLab.Data.Entities.Profile;

namespace MapLab.Web.Models.Profile
{
    public class ProfileViewModel : IMapFrom<ProfileEntity>
    {
        public string? UserName { get; set; }

        public string? ProfilePictureFilePath { get; set; }

        public ICollection<Map>? Maps { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsCurrentProfile { get; set; }

        public bool IsAdmin { get; set; }
    }
}
