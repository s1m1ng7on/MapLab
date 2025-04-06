using MapLab.Data.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Data.Repositories
{
    public class DeletableEntityRepository<TEntity> : Repository<TEntity>, IDeletableEntityRepository<TEntity> where TEntity : class, IAuditInfo, IDeletableEntity
    {
        public DeletableEntityRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor) { }

        public IQueryable<TEntity> AllWithDeleted() => base.All().IgnoreQueryFilters();

        public IQueryable<TEntity> AllAsNoTrackingWithDeleted() => AllWithDeleted().AsNoTracking();

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            Update(entity);
        }

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            Update(entity);
        }

        public void HardDelete(TEntity entity) => base.Delete(entity);
    }
}
