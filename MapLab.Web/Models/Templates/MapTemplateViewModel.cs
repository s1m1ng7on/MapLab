using MapLab.Data.Entities;
using MapLab.Services.Mapping;

namespace MapLab.Web.Models.Templates
{
    public class MapTemplateViewModel : IMapFrom<MapTemplate>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Region { get; set; }
        public string? ProfileUserName { get; set; }
    }
}
