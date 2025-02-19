using MapLab.Data.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MapLab.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IAuditInfo
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Repository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual IQueryable<TEntity> All() => _dbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => _dbSet.AsNoTracking();

        public virtual async Task<TEntity?> FindAsync(object id) => await _dbSet.FindAsync(id);

        public Task AddAsync(TEntity entity)
        {
            if (entity is IOwnable auditInfo)
            {
                auditInfo.ProfileId = GetCurrentUserId();
            }

            return _dbSet.AddAsync(entity).AsTask();
        }

        public void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
            entity.UpdatedOn = DateTime.UtcNow;
        }

        public virtual void Delete(TEntity entity) => _dbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }

        private string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
