﻿using XPlan.Entities;

namespace XPlan.DataAccess
{
    public interface IDataAccess<TEntity> where TEntity : IEntity
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<List<TEntity>> QueryAllAsync();
        Task<TEntity> QueryAsync(string key);
        Task<List<TEntity>> QueryAsync(List<string> key);
        Task<List<TEntity>> QueryByTimeAsync(DateTime? startTime, DateTime? endTime);
        Task<bool> UpdateAsync(string key, TEntity entity, List<string>? noUpdateLit = null);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task<bool> ExistsAsync(List<string> key);
        Task<TEntity> FindLastAsync();
    }
}
