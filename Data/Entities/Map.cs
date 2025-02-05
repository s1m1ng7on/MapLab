using MapLab.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class Map : DeletableEntity<string>, IOwnable
    {
        [Required]
        public string? Name { get; set; }

        [ForeignKey(nameof(MapTemplate))]
        [Required]
        public string? TemplateId { get; set; }

        [ForeignKey(nameof(Entities.Profile))]
        public string? ProfileId { get; set; }

        public virtual MapTemplate? Template { get; set; }

        public virtual Profile? Profile { get; set; }
    }
}
