using MapLab.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class Like<LikeableEntity> : DeletableEntity<string>, IOwnable where LikeableEntity : class
    {
        [ForeignKey(nameof(LikeableEntity))]
        [Required]
        public string? EntityId { get; set; }
        public virtual LikeableEntity? Entity { get; set; }

        [ForeignKey(nameof(Profile))]
        [Required]
        public string? ProfileId { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}
