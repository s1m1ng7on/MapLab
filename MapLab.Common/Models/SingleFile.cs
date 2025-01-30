using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Common.Models
{
    [NotMapped]
    public class SingleFile
    {
        [Required]
        public string? Path { get; set; }

        [NotMapped]
        [Required]
        public IFormFile? File { get; set; }

        public override string ToString() => Path ?? string.Empty;
    }
}
