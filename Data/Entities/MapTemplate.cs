using MapLab.Data.Models;
using Microsoft.AspNetCore.Http;
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

        [ForeignKey(nameof(Profile))]
        public string? ProfileId { get; set; }

        public virtual Profile? Profile { get; set; }

        public virtual ICollection<Map>? Maps { get; set; }
    }
}
