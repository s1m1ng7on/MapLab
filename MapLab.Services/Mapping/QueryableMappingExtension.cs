﻿using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;

namespace MapLab.Services.Mapping
{
    public static class QueryableMappingExtension
    {
        public static IQueryable<TDestination> To<TDestination>(this IQueryable source, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo(AutoMapperConfiguration.MapperInstance.ConfigurationProvider, null, membersToExpand);
        }

        public static IQueryable<TDestination> To<TDestination>(this IQueryable source, object parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(AutoMapperConfiguration.MapperInstance.ConfigurationProvider, parameters);
        }
    }
}
