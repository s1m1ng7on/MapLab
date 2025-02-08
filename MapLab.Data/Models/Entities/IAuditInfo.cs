namespace MapLab.Data.Models.Entities
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
    }
}
