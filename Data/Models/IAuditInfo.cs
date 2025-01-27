namespace MapLab.Data.Models
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
    }
}
