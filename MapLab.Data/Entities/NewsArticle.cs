using MapLab.Data.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapLab.Data.Entities
{
    public class NewsArticle : DeletableEntity<string>, IOwnable
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

        public string? ThumbnailFilePath { get; set; }

        [ForeignKey(nameof(Profile))]
        [Required]
        public string? ProfileId { get; set; }

        public virtual Profile? Profile { get; set; }
    }
}
