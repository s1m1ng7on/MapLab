using MapLab.Shared.Models.FilterModels;

namespace MapLab.Web.Models.Maps
{
    public class MapsIndexViewModel
    {
        public IEnumerable<MapViewModel>? Maps { get; set; }
        public string? ProfileUserName { get; set; }
        public bool IsCurrentProfile { get; set; }
        public MapFiltersModel? Filters { get; set; }
        public MapCreateViewModel? MapCreateViewModel { get; set; }
    }
}
