using MongoDB.Bson;
using MongoDB.Driver;
using XPlan.Entities;

namespace XPlan.DataAccess
{
    public interface IDataAccess<TEntity> where TEntity : IDBEntity
    {
        Task<TEntity?> InsertAsync(TEntity entity);
        Task<List<TEntity>?> QueryAllAsync();
        Task<TEntity?> QueryAsync(string key);
        Task<List<TEntity>?> QueryAsync(List<string> key);
        Task<List<TEntity>?> QueryByTimeAsync(DateTime? startTime, DateTime? endTime);
        Task<bool> UpdateAsync(string key, TEntity entity);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<bool> ExistsAsync(List<string> key);
        Task<TEntity?> FindLastAsync();
    }
}
