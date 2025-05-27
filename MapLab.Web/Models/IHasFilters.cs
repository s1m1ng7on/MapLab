namespace MapLab.Web.Models
{
    public interface IHasFilters<TFiltersModel>
    {
        public TFiltersModel? Filters { get; set; }
    }
}
