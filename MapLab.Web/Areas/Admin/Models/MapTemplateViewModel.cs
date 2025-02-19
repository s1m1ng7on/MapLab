using MapLab.Common.Attributes.Validation;
using MapLab.Data.Entities;
using MapLab.Data.Models.Enums;
using MapLab.Services.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Web.Areas.Admin.Models
{
    public class MapTemplateViewModel : IMapTo<MapTemplate>
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name must be between {2} and {1} characters.", MinimumLength = 3)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The Region field is required.")]
        public Region? Region { get; set; }

        [Required(ErrorMessage = "A file is required.")]
        [DataType(DataType.Upload)]
        [GeoJson]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "File size must not exceed 5MB.")]
        public IFormFile? File { get; set; }
    }
}
