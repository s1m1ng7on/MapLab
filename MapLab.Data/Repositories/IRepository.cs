namespace MapLab.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All();
        IQueryable<TEntity> AllAsNoTracking();
        IQueryable<TEntity> AllWithIncludes(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);
        Task<TEntity?> FindAsync(object id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
