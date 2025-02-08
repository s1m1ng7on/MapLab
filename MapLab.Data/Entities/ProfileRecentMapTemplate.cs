using MapLab.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class ProfileRecentMapTemplate : Entity<string>, IOwnable
    {
        [ForeignKey(nameof(Profile))]
        [Required]
        public string? ProfileId { get; set; }
        public virtual Profile? Profile { get; set; }

        [ForeignKey(nameof(MapTemplate))]
        [Required]
        public string? MapTemplateId { get; set; }

        public virtual MapTemplate? MapTemplate { get; set; }
    }
}
