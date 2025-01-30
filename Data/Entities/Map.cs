using MapLab.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class Map : DeletableEntity<string>, IOwnable
    {
        [Required]
        public string? Name { get; set; }

        [ForeignKey(nameof(MapTemplate))]
        [Required]
        public string? TemplateId { get; set; }

        [ForeignKey(nameof(Profile))]
        public string? CreatedByUserId { get; set; }

        public virtual MapTemplate? Template { get; set; }

        public virtual Profile? CreatedByUser { get; set; }
    }
}
