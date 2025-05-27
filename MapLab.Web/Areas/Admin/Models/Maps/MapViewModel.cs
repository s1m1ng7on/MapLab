using MapLab.Data.Models.Enums;
using MapLab.Services.Mapping;
using MapLab.Services.Models;

namespace MapLab.Web.Areas.Admin.Models.Maps
{
    public class MapViewModel : IMapFrom<MapDto>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool IsPublic { get; set; }
        public Region? MapTemplateRegion { get; set; }
        public string? ProfileId { get; set; }
        public string? ProfileUserName { get; set; }
        public string? ProfileProfilePictureFilePath { get; set; }
        public string? MapTemplateId { get; set; }
        public string? MapTemplateName { get; set; }
        public int LikesCount { get; set; }
        public int ViewsCount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
