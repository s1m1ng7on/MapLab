namespace MapLab.Web.Areas.Admin.Models
{
    public interface IHasFilters<TFiltersModel>
    {
        public TFiltersModel? Filters { get; set; }
    }
}
