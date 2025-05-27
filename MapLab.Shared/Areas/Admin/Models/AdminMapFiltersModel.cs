using MapLab.Shared.Models.FilterModels;

namespace MapLab.Shared.Areas.Admin.Models
{
    public class AdminMapFiltersModel: MapFiltersModel
    {
        public string? Search { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int ViewsMin { get; set; }
        public int ViewsMax { get; set; }
        public int LikesMin { get; set; }
        public int LikesMax { get; set; }
    }
}
