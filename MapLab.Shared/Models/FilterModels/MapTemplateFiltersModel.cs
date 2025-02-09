using MapLab.Data.Models.Enums;

namespace MapLab.Shared.Models.FilterModels
{
    public class MapTemplateFiltersModel
    {
        public string? SearchQuery { get; set; }
        public Region? Region { get; set; }
        public bool ByMapLab { get; set; }
    }
}
