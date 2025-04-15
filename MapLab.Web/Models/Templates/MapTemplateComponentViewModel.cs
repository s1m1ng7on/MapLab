using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Models.Templates
{
    public class MapTemplateComponentViewModel : IMapFrom<MapTemplateDto>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public string? MapTemplateJson { get; set; }
    }
}
