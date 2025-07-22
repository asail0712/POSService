using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;
using SharpCompress.Common;
using System.Collections.Generic;
using XPlan.DataAccess;
using XPlan.Entities;
using XPlan.Utility.Caches;

namespace XPlan.Repository
{
    public class GenericRepository<TEntity, TDataAccess>// : IRepository<TEntity> 
        where TEntity : IDBEntity where TDataAccess : IDataAccess<TEntity>
    {
        protected readonly TDataAccess _dataAccess;
        private readonly IMemoryCache _cache;
        private readonly int _cacheDurationMinutes;
        private readonly string _cachePrefix = typeof(TEntity).Name;

        private bool _bCacheEnable;

        public GenericRepository(TDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
        {
            _dataAccess             = dataAccess;
            _cache                  = memoryCache;
            _cacheDurationMinutes   = cacheSettings.Value.CacheDurationMinutes;
            _bCacheEnable           = cacheSettings.Value.CacheEnable;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            TEntity? result = await _dataAccess.InsertAsync(entity);

            if(result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            _cache.Set($"{_cachePrefix}:{result.Id}", result, TimeSpan.FromMinutes(_cacheDurationMinutes));
            _cache.Remove($"{_cachePrefix}:all");
            _cache.Remove($"{_cachePrefix}:findLast"); // 🆕 新增資料後，移除 FindLast 快取

            return result;
        }

        public virtual async Task<List<TEntity>> GetAllAsync(bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:all";

            if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out List<TEntity>? cachedList))
            {
                if (cachedList == null)
                {
                    throw new InvalidOperationException("");
                }

                return cachedList;
            }

            cachedList = await _dataAccess.QueryAllAsync();

            if (cachedList == null)
            {
                throw new ArgumentNullException(cacheKey);
            }

            _cache.Set(cacheKey, cachedList, TimeSpan.FromMinutes(_cacheDurationMinutes));

            return cachedList;
        }

        public virtual async Task<List<TEntity>> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null)
        {
            var result = await _dataAccess.QueryByTimeAsync(startTime, endTime);

            if(result == null)
            {
                throw new ArgumentNullException("");
            }

            return result;
        }

        public virtual async Task<TEntity> GetAsync(string key, bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:{key}";

            if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out TEntity? cachedEntity))
            {
                if (cachedEntity == null)
                {
                    throw new InvalidOperationException("");
                }

                return cachedEntity;
            }

            cachedEntity = await _dataAccess.QueryAsync(key);

            if (cachedEntity == null)
            {
                throw new ArgumentNullException(cacheKey);
            }

            _cache.Set(cacheKey, cachedEntity, TimeSpan.FromMinutes(_cacheDurationMinutes));

            return cachedEntity;
        }

        public virtual async Task<List<TEntity>> GetAsync(List<string> keys, bool bCache = true)
        {
            if (keys == null || keys.Count == 0)
            {
                throw new InvalidOperationException("");
            }

            var resultList          = new List<TEntity>();
            var keysToFetchFromDb   = new List<string>();

            if (_bCacheEnable && bCache)
            {
                foreach (var key in keys)
                {
                    var cacheKey = $"{_cachePrefix}:{key}";
                    if (_cache.TryGetValue(cacheKey, out TEntity? cachedEntity))
                    {
                        if (cachedEntity == null)
                        {
                            throw new InvalidOperationException("");
                        }

                        resultList.Add(cachedEntity);
                    }
                    else
                    {
                        keysToFetchFromDb.Add(key);
                    }
                }
            }
            else
            {
                keysToFetchFromDb = keys;
            }

            if (keysToFetchFromDb.Count > 0)
            {
                // 假設你的 IDataAccess<TEntity> 有支援批次查詢
                var dbEntities = await _dataAccess.QueryAsync(keysToFetchFromDb);

                if (dbEntities == null)
                {
                    throw new ArgumentException("");
                }

                foreach (var entity in dbEntities)
                {
                    if (entity != null)
                    {
                        var cacheKey = $"{_cachePrefix}:{entity.Id}";
                        _cache.Set(cacheKey, entity, TimeSpan.FromMinutes(_cacheDurationMinutes));
                        resultList.Add(entity);
                    }
                }
            }

            return resultList;
        }

        public virtual async Task UpdateAsync(string key, TEntity entity)
        {
            bool bResult = await _dataAccess.UpdateAsync(key, entity);

            if (bResult)
            {
                _cache.Set($"{_cachePrefix}:{key}", entity, TimeSpan.FromMinutes(_cacheDurationMinutes));
                _cache.Remove($"{_cachePrefix}:all");
                _cache.Remove($"{_cachePrefix}:exists:{key}");
                _cache.Remove($"{_cachePrefix}:findLast"); // 🆕 新增資料後，移除 FindLast 快取
            }
            else
            {
                throw new Exception("");
            }
        }

        public virtual async Task DeleteAsync(string key)
        {
            bool bResult =await _dataAccess.DeleteAsync(key);

            if (bResult)
            {
                _cache.Remove($"{_cachePrefix}:{key}");
                _cache.Remove($"{_cachePrefix}:all");
                _cache.Remove($"{_cachePrefix}:exists:{key}");
                _cache.Remove($"{_cachePrefix}:findLast"); // 🆕 新增資料後，移除 FindLast 快取
            }
            else
            {
                throw new Exception("");
            }
        }
        public virtual async Task<bool> ExistsAsync(string key, bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:exists:{key}";

            if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out bool cachedExists))
            {
                return cachedExists;
            }

            bool exists = await _dataAccess.ExistsAsync(key);

            _cache.Set(cacheKey, exists, TimeSpan.FromMinutes(_cacheDurationMinutes));

            return exists;
        }

        public virtual async Task<bool> ExistsAsync(List<string> keys, bool bCache = true)
        {
            if (keys == null || keys.Count == 0)
            { 
                return false;
            }

            // 批次快取檢查（全部 keys 都有快取時直接回傳）
            var missingKeys = new List<string>();
            if (_bCacheEnable && bCache)
            {
                foreach (var key in keys)
                {
                    var cacheKey = $"{_cachePrefix}:exists:{key}";
                    if (!_cache.TryGetValue(cacheKey, out bool cachedExists) || !cachedExists)
                    {
                        missingKeys.Add(key);
                    }
                }

                if (missingKeys.Count == 0)
                {
                    return true; // 全部 keys 都在快取且存在
                }
            }
            else
            {
                missingKeys = keys;
            }

            // 用批次查詢檢查缺少的 keys
            bool allExist = await _dataAccess.ExistsAsync(missingKeys);
            foreach (var key in missingKeys)
            {
                _cache.Set($"{_cachePrefix}:exists:{key}", allExist, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }

            return allExist;
        }

        public virtual async Task<TEntity> FindLastAsync(bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:findLast";

            // 嘗試從快取取得
            if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out TEntity? cachedEntity))
            {
                if (cachedEntity == null)
                {
                    throw new InvalidOperationException("");
                }

                return cachedEntity;
            }

            // 如果快取沒有，從資料庫查詢
            var lastEntity = await _dataAccess.FindLastAsync();

            if (lastEntity != null)
            {
                // 寫入快取，設定過期時間（例如 30 秒）
                _cache.Set(cacheKey, lastEntity, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }
            else
            {
                throw new InvalidOperationException("");
            }

            return lastEntity;
        }
    }
}
