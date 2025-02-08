using MapLab.Common.Models;
using MapLab.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Data.Entities
{
    public class Profile : IdentityUser, IAuditInfo
    {
        [Required]
        public SingleFile? ProfilePicture { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<Map>? Maps { get; set; }
        public virtual ICollection<MapTemplate>? MapTemplates { get; set; }
        public virtual ICollection<ProfileRecentMapTemplate>? ProfileRecentMapTemplates { get; set; }
    }
}
