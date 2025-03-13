using MapLab.Data.Entities;
using MapLab.Services.Mapping;

namespace MapLab.Web.Models.Maps
{
    public class MapViewModel : IMapFrom<Map>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ThumbnailFilePath { get; set; }
        public string? TemplateName { get; set; }
        public bool IsPublic { get; set; }
        public string? ProfileUserName { get; set; }
    }
}
