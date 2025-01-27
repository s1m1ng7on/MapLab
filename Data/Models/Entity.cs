using System.ComponentModel.DataAnnotations;

namespace MapLab.Data.Models
{
    public abstract class Entity<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }
    }
}
