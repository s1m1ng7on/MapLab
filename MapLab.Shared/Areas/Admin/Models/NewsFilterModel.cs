namespace MapLab.Shared.Areas.Admin.Models
{
    public class NewsFilterModel
    {
        public string? Search { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? QuickDateFilter { get; set; }
    }
}
