using MapLab.Shared.Models.FilterModels;

namespace MapLab.Shared.Areas.Admin.Models
{
    public class AdminTemplateFiltersModel : MapTemplateFiltersModel
    {
        public string? Search { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
