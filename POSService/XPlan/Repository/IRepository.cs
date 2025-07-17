using XPlan.Entity;

namespace XPlan.Repository
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task CreateAsync(TEntity entity);
        Task<List<TEntity>?> GetAllAsync(bool bCache = true);
        Task<TEntity?> GetAsync(string key, bool bCache = true);
        Task<List<TEntity>?> GetAsync(List<string> key, bool bCache = true);
        Task<List<TEntity>?> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null);
        Task<bool> UpdateAsync(string key, TEntity entity);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key, bool bCache = true);
        Task<bool> ExistsAsync(List<string> keys, bool bCache = true);
    }
}

