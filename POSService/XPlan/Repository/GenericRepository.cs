using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using SharpCompress.Common;
using System.Collections.Generic;
using XPlan.DataAccess;
using XPlan.Interface;
using XPlan.Utility.Caches;

namespace XPlan.Repository
{
    public class GenericRepository<TEntity, TDataAccess> : IRepository<TEntity> where TEntity : class, IEntity where TDataAccess : IDataAccess<TEntity>
    {
        protected readonly TDataAccess _dataAccess;
        private readonly IMemoryCache _cache;
        private readonly int _cacheDurationMinutes;
        private readonly string _cachePrefix = typeof(TEntity).Name;

        public GenericRepository(TDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
        {
            _dataAccess             = dataAccess;
            _cache                  = memoryCache;
            _cacheDurationMinutes   = cacheSettings.Value.CacheDurationMinutes;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dataAccess.InsertAsync(entity);

            _cache.Set($"{_cachePrefix}:{entity.Id}", entity, TimeSpan.FromMinutes(_cacheDurationMinutes));
            _cache.Remove($"{_cachePrefix}:all");
        }

        public async Task<List<TEntity>?> GetAllAsync(bool bCache = true)
        {
            List<TEntity>? cachedList   = null;
            var cacheKey                = $"{_cachePrefix}:all";

            if (bCache && _cache.TryGetValue(cacheKey, out cachedList))
            {
                return cachedList;
            }

            cachedList = await _dataAccess.QueryAllAsync();

            _cache.Set(cacheKey, cachedList, TimeSpan.FromMinutes(_cacheDurationMinutes));

            return cachedList;
        }

        public async Task<List<TEntity>?> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null)
        {
            return await _dataAccess.QueryByTimeAsync(startTime, endTime);
        }

        public async Task<TEntity?> GetAsync(string key, bool bCache = true)
        {
            TEntity? cachedEntity   = null;
            var cacheKey            = $"{_cachePrefix}:{key}";

            if (bCache && _cache.TryGetValue(cacheKey, out cachedEntity))
            {
                return cachedEntity;
            }

            cachedEntity = await _dataAccess.QueryAsync(key);

            if (cachedEntity != null)
            {
                _cache.Set(cacheKey, cachedEntity, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }

            return cachedEntity;
        }

        public async Task<bool> UpdateAsync(string key, TEntity entity)
        {
            bool bResult = await _dataAccess.UpdateAsync(key, entity);

            if (bResult)
            {
                _cache.Set($"{_cachePrefix}:{key}", entity, TimeSpan.FromMinutes(5));
                _cache.Remove($"{_cachePrefix}:all");
                _cache.Remove($"{_cachePrefix}:exists:{key}");
            }

            return bResult;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            bool bResult =await _dataAccess.DeleteAsync(key);

            if (bResult)
            {
                _cache.Remove($"{_cachePrefix}:{key}");
                _cache.Remove($"{_cachePrefix}:all");
                _cache.Remove($"{_cachePrefix}:exists:{key}");
            }

            return bResult;
        }
        public async Task<bool> ExistsAsync(string key, bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:exists:{key}";

            if (bCache && _cache.TryGetValue(cacheKey, out bool cachedExists))
            {
                return cachedExists;
            }

            bool exists = await _dataAccess.ExistsAsync(key);

            _cache.Set(cacheKey, exists, TimeSpan.FromMinutes(_cacheDurationMinutes));

            return exists;
        }
    }
}
