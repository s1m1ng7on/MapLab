using MapLab.Services.Mapping;
using MapLab.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace MapLab.Web.Models.Maps
{
    public class EditMapViewModel : IMapTo<MapDto>
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        public string? Name { get; set; }

        public bool IsPublic { get; set; }
    }
}
