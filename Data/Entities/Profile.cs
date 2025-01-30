using MapLab.Common.Models;
using MapLab.Data.Models;
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
    }
}
