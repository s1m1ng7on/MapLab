using MapLab.Data.Models.Enums;
using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Areas.Admin.Models.Templates
{
    public class TemplateViewModel : IMapFrom<MapTemplateDto>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public Region? Region { get; set; }
        public int MapsCount { get; set; }
        public int LikesCount { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
