using MapLab.Data.Entities;
using MapLab.Data.Models.Enums;
using MapLab.Services.Mapping;

namespace MapLab.Services.Models
{
    public class MapTemplateDto : IMapFrom<MapTemplate>, IMapTo<MapTemplate>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public Region Region { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public string? FilePath { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
