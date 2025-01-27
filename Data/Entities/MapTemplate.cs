using MapLab.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class MapTemplate : DeletableEntity<string>, IOwnable
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? File { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        public string? CreatedByUserId { get; set; }

        public virtual IdentityUser? CreatedByUser { get; set; }
    }
}
