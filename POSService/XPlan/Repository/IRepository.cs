﻿using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using XPlan.Entities;

namespace XPlan.Repository
{
    // 泛型資料存取介面，適用於所有實作 IDBEntity 的實體
    public interface IRepository<TEntity> where TEntity : IDBEntity
    {
        // 建立新資料
        Task<TEntity> CreateAsync(TEntity entity);

        // 取得所有資料（可選擇是否使用快取）
        Task<List<TEntity>> GetAllAsync(bool bCache = true);

        // 根據唯一 Key 取得單筆資料（可選擇是否使用快取）
        Task<TEntity> GetAsync(string key, bool bCache = true);

        // 根據多個 Key 取得多筆資料（可選擇是否使用快取）
        Task<List<TEntity>> GetAsync(List<string> key, bool bCache = true);

        // 更新指定 Key 的資料
        Task UpdateAsync(string key, TEntity entity);

        // 刪除指定 Key 的資料
        Task DeleteAsync(string key);

        // 取得指定時間範圍內的資料
        Task<List<TEntity>> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null);

        // 判斷單一 Key 是否存在（可選擇是否使用快取）
        Task<bool> ExistsAsync(string key, bool bCache = true);

        // 判斷多個 Key 是否存在（可選擇是否使用快取）
        Task<bool> ExistsAsync(List<string> keys, bool bCache = true);

        // 取得最後一筆更新的資料（可選擇是否使用快取）
        Task<TEntity> FindLastAsync(bool bCache = true);

        // 使用條件式查詢符合條件的資料
        Task<List<TEntity>?> GetAsync(Expression<Func<TEntity, bool>> predicate);

        // 查詢並對符合條件的資料進行更新邏輯處理（例如：邏輯刪除、狀態變更等）
        Task<List<TEntity>?> QueryAndUpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction);
    }
}
