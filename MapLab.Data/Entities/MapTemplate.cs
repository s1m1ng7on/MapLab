using MapLab.Data.Models.Entities;
using MapLab.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class MapTemplate : DeletableEntity<string>, IOwnable
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public Region Region { get; set; }

        public string? ThumbnailFilePath { get; set; }

        public string? FilePath { get; set; }

        [ForeignKey(nameof(Profile))]
        public string? ProfileId { get; set; }

        public virtual Profile? Profile { get; set; }

        public virtual ICollection<Map>? Maps { get; set; }
    }
}
