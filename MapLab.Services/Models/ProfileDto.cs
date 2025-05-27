using MapLab.Data.Entities;
using MapLab.Services.Mapping;

namespace MapLab.Services.Models
{
    public class ProfileDto : IMapFrom<Profile>
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureFilePath { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<MapDto>? Maps { get; set; }
        public virtual ICollection<MapTemplateDto>? MapTemplates { get; set; }
        public virtual ICollection<MapView>? MapViews { get; set; }
    }
}
