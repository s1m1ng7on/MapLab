using MapLab.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MapLab.Data.Configurations
{
    public static class GlobalFilters
    {
        public static void ConfigureDeletableEntityFilters(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var filter = Expression.Lambda(
                        Expression.Not(Expression.Property(parameter, "IsDeleted")),
                        parameter
                    );
                    builder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }
    }
}