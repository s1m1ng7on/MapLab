using MapLab.Data.Entities;

namespace MapLab.Data.Models
{
    public interface IOwnable
    {
        public string? CreatedByUserId { get; set; }
        public Profile? CreatedByUser { get; set; }
    }
}
