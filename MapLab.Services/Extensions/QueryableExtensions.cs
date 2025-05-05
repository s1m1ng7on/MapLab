using AutoMapper;
using AutoMapper.QueryableExtensions;
using MapLab.Services.Models;

namespace MapLab.Services.Extensions
{
    public static class QueryableExtensions
    {
        public static PaginationDto<TDestination> ToPaginationDto<TSource, TDestination>(
            this IQueryable<TSource> query,
            IMapper mapper,
            int page,
            int pageSize)
        {
            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TDestination>(mapper.ConfigurationProvider)
                .ToList();

            return new PaginationDto<TDestination>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public static PaginationDto<TDto> ToPaginationDto<TDto>(
        this IEnumerable<TDto> items,
        int page = 1,
        int pageSize = 10)
        {
            var totalCount = items.Count();
            var paginatedItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationDto<TDto>
            {
                Items = paginatedItems,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }
    }
}
