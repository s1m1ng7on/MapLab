using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Common.Models
{
    public class SingleFile
    {
        [Required]
        public string? FilePath { get; set; }

        [NotMapped]
        [Required]
        public IFormFile File { get; set; }

        public override string ToString() => FilePath ?? string.Empty;
    }
}
