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
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IDataAccess<TEntity> _dataAccess;
        private readonly IMemoryCache _cache;
        private readonly int _cacheDurationMinutes;
        private readonly string _cachePrefix = typeof(TEntity).Name;

        public GenericRepository(IDataAccess<TEntity> dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
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

        public async Task<IEnumerable<TEntity?>?> GetAllAsync(bool bCache = true)
        {
            IEnumerable<TEntity?>? cachedList   = null;
            var cacheKey                        = $"{_cachePrefix}:all";

            if (bCache && _cache.TryGetValue(cacheKey, out cachedList))
            {
                return cachedList;
            }

            cachedList = await _dataAccess.QueryAllAsync();

            _cache.Set(cacheKey, cachedList, TimeSpan.FromMinutes(_cacheDurationMinutes));

            return cachedList;
        }

        public async Task<TEntity?> GetByIdAsync(string id, bool bCache = true)
        {
            TEntity? cachedEntity   = null;
            var cacheKey            = $"{_cachePrefix}:{id}";

            if (bCache && _cache.TryGetValue(cacheKey, out cachedEntity))
            {
                return cachedEntity;
            }

            cachedEntity = await _dataAccess.QueryByIdAsync(id);

            if (cachedEntity != null)
            {
                _cache.Set(cacheKey, cachedEntity, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }

            return cachedEntity;
        }

        public async Task<bool> UpdateAsync(string id, TEntity entity)
        {
            entity.Id       = new ObjectId(id);             // Ensure the ID is set for the update operation
            bool bResult    = await _dataAccess.UpdateAsync(id, entity);

            if (bResult)
            {
                _cache.Set($"{_cachePrefix}:{id}", entity, TimeSpan.FromMinutes(5));
                _cache.Remove($"{_cachePrefix}:all");
            }

            return bResult;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool bResult =await _dataAccess.DeleteAsync(id);

            if (bResult)
            {
                _cache.Remove($"{_cachePrefix}:{id}");
                _cache.Remove($"{_cachePrefix}:all");
            }

            return bResult;
        }
    }
}
