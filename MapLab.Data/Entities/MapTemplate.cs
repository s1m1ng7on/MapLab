using MapLab.Common.Models;
using MapLab.Data.Models.Entities;
using MapLab.Data.Models.Enums;
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
        public Region Region { get; set; }

        [Required]
        public SingleFile? File { get; set; }

        [ForeignKey(nameof(Profile))]
        public string? ProfileId { get; set; }

        public virtual Profile? Profile { get; set; }

        public virtual ICollection<Map>? Maps { get; set; }

        public virtual ICollection<ProfileRecentMapTemplate>? ProfileRecentMapTemplates { get; set; }
    }
}
