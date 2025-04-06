using MapLab.Data.Models.Enums;
using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Models.Templates
{
    public class MapTemplateViewModel : IMapFrom<MapTemplateDto>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public Region? Region { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
