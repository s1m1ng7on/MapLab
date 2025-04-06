using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Models.Maps
{
    public class MapComponentViewModel : IMapFrom<MapDto>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool CanEdit { get; set; }
        public string? TemplateId { get; set; }
        public string? TemplateName { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public string? MapTemplateJson { get; set; }
        public string? MapJson { get; set; }
    }
}
