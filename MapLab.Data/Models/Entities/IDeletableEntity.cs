namespace MapLab.Data.Models.Entities
{
    public interface IDeletableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
