namespace XPlan.Interface
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity?> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity?>?> GetAllAsync();
        Task<TEntity?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, TEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}

