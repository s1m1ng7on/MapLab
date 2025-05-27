namespace MapLab.Shared.Areas.Admin.Models
{
    public class ProfileFiltersModel
    {
        public string? Search { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsAdmin { get; set; }
    }
}
