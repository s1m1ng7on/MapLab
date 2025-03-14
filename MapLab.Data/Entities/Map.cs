using MapLab.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class Map : DeletableEntity<string>, IOwnable
    {
        [Required]
        public string? Name { get; set; }

        public string? ThumbnailFilePath { get; set; }

        public string? FilePath { get; set; }

        [ForeignKey(nameof(Template))]
        [Required]
        public string? TemplateId { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [ForeignKey(nameof(Profile))]
        [Required]
        public string? ProfileId { get; set; }

        public virtual MapTemplate? Template { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual ICollection<MapView>? MapViews { get; set; }
    }
}
