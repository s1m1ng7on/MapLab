using MapLab.Data.Entities;

namespace MapLab.Data.Models.Entities
{
    public interface IOwnable
    {
        public string? ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
