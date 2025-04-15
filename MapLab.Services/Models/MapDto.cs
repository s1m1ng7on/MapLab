using MapLab.Data.Entities;
using MapLab.Services.Mapping;

namespace MapLab.Services.Models
{
    public class MapDto : IMapFrom<Map>, IMapTo<Map>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public string? FilePath { get; set; }
        public string? MapTemplateId { get; set; }
        public bool IsPublic { get; set; }
        public string? ProfileId { get; set; }
        public virtual MapTemplateDto? MapTemplate { get; set; }
        public virtual Profile? Profile { get; set; }
        public virtual ICollection<MapView>? Views { get; set; }
        public virtual ICollection<Like<Map>>? Likes { get; set; }
    }
}
