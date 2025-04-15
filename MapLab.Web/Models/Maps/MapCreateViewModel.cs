using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Web.Models.Templates;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Web.Models.Maps
{
    public class MapCreateViewModel : IMapTo<MapDto>
    {
        [Required(ErrorMessage = "Please enter a name.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please select a map template.")]
        public string? MapTemplateId { get; set; }

        public bool IsPublic { get; set; }

        public List<MapTemplateViewModel>? RecentMapTemplates { get; set; }
        public List<MapTemplateViewModel>? MapLabMapTemplates { get; set; }
        public List<MapTemplateViewModel>? FeaturedMapTemplates { get; set; }
    }
}
