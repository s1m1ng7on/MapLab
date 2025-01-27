using Microsoft.AspNetCore.Identity;

namespace MapLab.Data.Models
{
    public interface IOwnable
    {
        public string? CreatedByUserId { get; set; }
        public IdentityUser? CreatedByUser { get; set; }
    }
}
