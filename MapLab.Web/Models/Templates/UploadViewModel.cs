using MapLab.Common.Attributes.Validation;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Web.Models.Templates
{
    public class UploadViewModel
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [FileExtension(".json", ".geojson")]
        public IFormFile? File { get; set; }
    }
}
