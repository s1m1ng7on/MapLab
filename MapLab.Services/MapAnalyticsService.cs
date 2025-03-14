using MapLab.Data.Entities;
using MapLab.Data.Repositories;
using MapLab.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace MapLab.Services
{
    public class MapAnalyticsService : IMapAnalyticsService
    {
        private readonly IDeletableEntityRepository<Map> _mapRepository;
        private readonly IRepository<MapView> _mapViewRepository;
        private readonly IMemoryCache _memoryCache;

        private const string TopMapsForWeekCacheKey = "TopMapsForWeek";

        public MapAnalyticsService(IDeletableEntityRepository<Map> mapRepository, IRepository<MapView> mapViewRepository, IMemoryCache memoryCache)
        {
            _mapRepository = mapRepository;
            _mapViewRepository = mapViewRepository;
            _memoryCache = memoryCache;
        }

        public IEnumerable<Map>? GetTopMapsForWeek()
        {
            if (!_memoryCache.TryGetValue(TopMapsForWeekCacheKey, out IEnumerable<Map>? cachedMaps))
            {
                var lastMonday = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
                var lastSunday = lastMonday.AddDays(6);

                var topMaps = _mapViewRepository.All()
                    .Where(mv => !mv.CanEdit && mv.CreatedOn >= lastMonday && mv.CreatedOn <= lastSunday)
                    .GroupBy(mv => mv.MapId)
                    .Select(group => new
                    {
                        MapId = group.Key,
                        ViewCount = group.Count()
                    })
                    .OrderByDescending(x => x.ViewCount)
                    .Take(10)
                    .ToList();

                var mapIds = topMaps.Select(t => t.MapId).ToList();

                cachedMaps = _mapRepository.All()
                    .Where(m => mapIds.Contains(m.Id))
                    .ToList();

                var nextMonday = DateTime.Now.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek + 7) % 7).Date;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(nextMonday);

                _memoryCache.Set(TopMapsForWeekCacheKey, cachedMaps, cacheEntryOptions);
            }

            return cachedMaps;
        }
    }
}
