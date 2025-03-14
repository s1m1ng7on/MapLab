using MapLab.Data.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class MapView : Entity<string>, IOwnable
    {
        [ForeignKey(nameof(Map))] public string? MapId { get; set; }
        public virtual Map? Map { get; set; }
        [ForeignKey(nameof(Profile))] public string? ProfileId { get; set; }
        public virtual Profile? Profile { get; set; }
        public bool CanEdit { get; set; }
    }
}
