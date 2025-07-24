using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using XPlan.Entities;

namespace XPlan.Repository
{
    public interface IRepository<TEntity> where TEntity : IDBEntity
    {
        // 基本
        Task<TEntity> CreateAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync(bool bCache = true);
        Task<TEntity> GetAsync(string key, bool bCache = true);
        Task<List<TEntity>> GetAsync(List<string> key, bool bCache = true);
        Task UpdateAsync(string key, TEntity entity);
        Task DeleteAsync(string key);

        // 其他
        Task<List<TEntity>> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null);
        Task<bool> ExistsAsync(string key, bool bCache = true);
        Task<bool> ExistsAsync(List<string> keys, bool bCache = true);
        Task<TEntity> FindLastAsync(bool bCache = true);

        // Expression
        Task<List<TEntity>?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>?> QueryAndUpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction);
    }
}

