using MapLab.Data.Entities;
using MapLab.Services.Mapping;
using MapLab.Web.Models.Maps;
using MapLab.Web.Models.Templates;
using ProfileEntity = MapLab.Data.Entities.Profile;

namespace MapLab.Web.Models.Profile
{
    public class ProfileViewModel : IMapFrom<ProfileEntity>
    {
        public string? UserName { get; set; }

        public string? Bio { get; set; }

        public string? ProfilePictureFilePath { get; set; }

        public IEnumerable<MapViewModel>? Maps { get; set; }
        public IEnumerable<MapTemplateViewModel>? MapTemplates { get; set; }
        public IEnumerable<MapViewModel>? FavoriteMaps { get; set; }
        public IEnumerable<MapTemplateViewModel>? FavoriteMapTemplates { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsCurrentProfile { get; set; }

        public bool IsAdmin { get; set; }
    }
}
