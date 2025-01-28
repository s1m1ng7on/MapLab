using MapLab.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public SingleFile? ProfilePicture { get; set; }
    }
}
