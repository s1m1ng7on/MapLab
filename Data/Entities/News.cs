using MapLab.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Data.Entities
{
    public class News : DeletableEntity<string>
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }
    }
}
