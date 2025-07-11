namespace XPlan.Interface
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity?>?> GetAllAsync(bool bCache = true);
        Task<TEntity?> GetByIdAsync(string id, bool bCache = true);
        Task<bool> UpdateAsync(string id, TEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}

