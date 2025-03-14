using MapLab.Data.Entities;

namespace MapLab.Services.Contracts
{
    public interface IMapAnalyticsService
    {
        public IEnumerable<Map>? GetTopMapsForWeek();
    }
}
