using MongoDB.Bson;
using MongoDB.Driver;
using XPlan.Entities;

namespace XPlan.Repository
{
    public interface IRepository<TEntity> where TEntity : IDBEntity
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync(bool bCache = true);
        Task<TEntity> GetAsync(string key, bool bCache = true);
        Task<List<TEntity>> GetAsync(List<string> key, bool bCache = true);
        Task<List<TEntity>> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null);
        Task UpdateAsync(string key, TEntity entity);
        Task DeleteAsync(string key);
        Task<bool> ExistsAsync(string key, bool bCache = true);
        Task<bool> ExistsAsync(List<string> keys, bool bCache = true);
        Task<TEntity> FindLastAsync(bool bCache = true);
    }
}

