using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Cache;
using XPlan.Interface;

namespace XPlan.DataAccess
{
    public abstract class GenericDataAccess<TEntity> : IDataAccess<TEntity> where TEntity : class, IEntity
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IMemoryCache _cache;
        private readonly int _cacheDurationMinutes;
        private readonly string _cachePrefix = typeof(TEntity).Name;

        public GenericDataAccess(IMongoDatabase database, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings)
        {
            _collection             = database.GetCollection<TEntity>(typeof(TEntity).Name);
            _cache                  = memoryCache;
            _cacheDurationMinutes   = cacheSettings.Value.CacheDurationMinutes;
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);

            _cache.Set($"{_cachePrefix}:{entity.Id}", entity, TimeSpan.FromMinutes(_cacheDurationMinutes));
            _cache.Remove($"{_cachePrefix}:all");
        }

        public async Task<IEnumerable<TEntity?>?> QueryAllAsync()
        {
            var cacheKey = $"{_cachePrefix}:all";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<TEntity>? cachedList))
            {
                return cachedList;
            }

            var list = await _collection.Find(_ => true).ToListAsync();
            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(_cacheDurationMinutes));
            return list;
        }

        public async Task<TEntity?> QueryByIdAsync(string id)
        {
            var cacheKey = $"{_cachePrefix}:{id}";

            if (_cache.TryGetValue(cacheKey, out TEntity? cachedEntity))
            {
                return cachedEntity;
            }

            var entity = await _collection.Find(e => e.Id == new ObjectId(id)).FirstOrDefaultAsync();
            if (entity != null)
            {
                _cache.Set(cacheKey, entity, TimeSpan.FromMinutes(_cacheDurationMinutes));
            }
            return entity;
        }

        public async Task<bool> UpdateAsync(string id, TEntity entity)
        {
            var result = await _collection.ReplaceOneAsync(e => e.Id == new ObjectId(id), entity);

            if (result.ModifiedCount > 0)
            {
                _cache.Set($"{_cachePrefix}:{id}", entity, TimeSpan.FromMinutes(5));
                _cache.Remove($"{_cachePrefix}:all");
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(e => e.Id == new ObjectId(id));

            if (result.DeletedCount > 0)
            {
                _cache.Remove($"{_cachePrefix}:{id}");
                _cache.Remove($"{_cachePrefix}:all");
                return true;
            }
            return false;
        }
    }
}
