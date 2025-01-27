namespace MapLab.Data.Models
{
    public class DeletableEntity<TKey> : Entity<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
