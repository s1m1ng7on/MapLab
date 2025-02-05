using MapLab.Data.Entities;

namespace MapLab.Data.Models
{
    public interface IOwnable
    {
        public string? ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
