using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using XPlan.DataAccess;
using XPlan.Entities;
using XPlan.Utility.Caches;
using XPlan.Utility.Exceptions;

namespace XPlan.Repository
{
    public class GenericRepository<TEntity, TDataAccess>
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
            try
            {
                TEntity? result = await _dataAccess.InsertAsync(entity);

                if (result == null)
                {
                    throw new InvalidEntityException(typeof(TEntity).Name);
                }

                _cache.Set($"{_cachePrefix}:{result.Id}", result, TimeSpan.FromMinutes(_cacheDurationMinutes));
                _cache.Remove($"{_cachePrefix}:all");
                _cache.Remove($"{_cachePrefix}:findLast");

                return result;
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Create", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task<List<TEntity>> GetAllAsync(bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:all";

            try
            {
                if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out List<TEntity>? cachedList))
                {
                    if (cachedList == null)
                    {
                        throw new CacheMissException(cacheKey);
                    }

                    return cachedList;
                }

                var list = await _dataAccess.QueryAllAsync();

                if (list == null)
                {
                    throw new EntityNotFoundException(typeof(TEntity).Name, "All");
                }

                _cache.Set(cacheKey, list, TimeSpan.FromMinutes(_cacheDurationMinutes));

                return list;
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("GetAll", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task<TEntity> GetAsync(string key, bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:{key}";

            try
            {
                if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out TEntity? cachedEntity))
                {
                    if (cachedEntity == null)
                    {
                        throw new CacheMissException(cacheKey);
                    }

                    return cachedEntity;
                }

                var entity = await _dataAccess.QueryAsync(key);

                if (entity == null)
                {
                    throw new EntityNotFoundException(typeof(TEntity).Name, key);
                }

                _cache.Set(cacheKey, entity, TimeSpan.FromMinutes(_cacheDurationMinutes));

                return entity;
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Get", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task<List<TEntity>> GetAsync(List<string> keys, bool bCache = true)
        {
            if (keys == null || keys.Count == 0)
            {
                throw new InvalidRepositoryArgumentException(nameof(keys), "Keys list cannot be null or empty");
            }

            var resultList          = new List<TEntity>();
            var keysToFetchFromDb   = new List<string>();

            try
            {
                if (_bCacheEnable && bCache)
                {
                    foreach (var key in keys)
                    {
                        var cacheKey = $"{_cachePrefix}:{key}";
                        if (_cache.TryGetValue(cacheKey, out TEntity? cachedEntity))
                        {
                            if (cachedEntity == null)
                            {
                                throw new CacheMissException(cacheKey);
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
                    // 📝 假設 _dataAccess 支援批次查詢
                    var dbEntities = await _dataAccess.QueryAsync(keysToFetchFromDb);

                    if (dbEntities == null || dbEntities.Count == 0)
                    {
                        throw new EntityNotFoundException(typeof(TEntity).Name, string.Join(", ", keysToFetchFromDb));
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

                if (resultList.Count != keys.Count)
                {
                    // 檢查是否有缺失
                    var missingKeys = keys.Except(resultList.Select(e => e.Id)).ToList();

                    if (missingKeys.Any())
                    {
                        throw new EntityNotFoundException(typeof(TEntity).Name, string.Join(", ", missingKeys));
                    }
                }

                return resultList;
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Get(List)", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task<List<TEntity>> GetByTimeAsync(DateTime? startTime = null, DateTime? endTime = null)
        {
            if (startTime.HasValue && endTime.HasValue && startTime > endTime)
            {
                throw new InvalidRepositoryArgumentException(
                    "startTime, endTime",
                    "Start time cannot be later than end time"
                );
            }

            try
            {
                var result = await _dataAccess.QueryByTimeAsync(startTime, endTime);

                if (result == null || result.Count == 0)
                {
                    string rangeDescription = (startTime.HasValue && endTime.HasValue)
                        ? $"{startTime.Value:yyyy-MM-dd HH:mm:ss} ~ {endTime.Value:yyyy-MM-dd HH:mm:ss}"
                        : "specified time range";

                    throw new EntityNotFoundException(typeof(TEntity).Name, rangeDescription);
                }

                return result;
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("GetByTime", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task UpdateAsync(string key, TEntity entity)
        {
            try
            {
                bool bResult = await _dataAccess.UpdateAsync(key, entity);

                if (!bResult)
                {
                    throw new EntityNotFoundException(typeof(TEntity).Name, key);
                }

                _cache.Set($"{_cachePrefix}:{key}", entity, TimeSpan.FromMinutes(_cacheDurationMinutes));
                _cache.Remove($"{_cachePrefix}:all");
                _cache.Remove($"{_cachePrefix}:exists:{key}");
                _cache.Remove($"{_cachePrefix}:findLast");
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Update", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task DeleteAsync(string key)
        {
            try
            {
                bool bResult = await _dataAccess.DeleteAsync(key);

                if (!bResult)
                {
                    throw new EntityNotFoundException(typeof(TEntity).Name, key);
                }

                _cache.Remove($"{_cachePrefix}:{key}");
                _cache.Remove($"{_cachePrefix}:all");
                _cache.Remove($"{_cachePrefix}:exists:{key}");
                _cache.Remove($"{_cachePrefix}:findLast");
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Delete", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task<bool> ExistsAsync(string key, bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:exists:{key}";

            if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out bool cachedExists))
            {
                return cachedExists;
            }

            try
            {
                bool exists = await _dataAccess.ExistsAsync(key);

                _cache.Set(cacheKey, exists, TimeSpan.FromMinutes(_cacheDurationMinutes));

                return exists;
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Exist", typeof(TEntity).Name, ex);
            }
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

            try
            {
                // 用批次查詢檢查缺少的 keys
                bool allExist = await _dataAccess.ExistsAsync(missingKeys);

                foreach (var key in missingKeys)
                {
                    _cache.Set($"{_cachePrefix}:exists:{key}", allExist, TimeSpan.FromMinutes(_cacheDurationMinutes));
                }

                return allExist;
            }
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("Exists", typeof(TEntity).Name, ex);
            }
        }

        public virtual async Task<TEntity> FindLastAsync(bool bCache = true)
        {
            var cacheKey = $"{_cachePrefix}:findLast";

            try
            {
                // 嘗試從快取取得
                if (_bCacheEnable && bCache && _cache.TryGetValue(cacheKey, out TEntity? cachedEntity))
                {
                    if (cachedEntity == null)
                    {
                        throw new CacheMissException(cacheKey);
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
            catch (RepositoryException)
            {
                throw; // 不包自家 RepositoryException
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException("FindLastAsync", typeof(TEntity).Name, ex);
            }
        }
    }
}
