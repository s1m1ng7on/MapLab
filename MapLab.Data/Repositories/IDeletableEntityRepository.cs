using MapLab.Data.Models.Entities;
using System.Linq.Expressions;

namespace MapLab.Data.Repositories
{
    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();
        IQueryable<TEntity> AllAsNoTrackingWithDeleted();
        void HardDelete(TEntity entity);
        void Undelete(TEntity entity);
    }
}
