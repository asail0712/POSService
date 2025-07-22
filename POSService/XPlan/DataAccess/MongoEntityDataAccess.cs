using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Entities;

using XPlan.Entities;

namespace XPlan.DataAccess
{
    public abstract class MongoEntityDataAccess<TEntity, TDocument>
        where TEntity : class, IDBEntity, new()
        where TDocument : IEntity, IDBEntity, new() // 👈 注意繼承 Entity
    {
        private readonly IMapper _mapper;
        private static bool _bIndexCreated  = false;
        private static string _searchKey    = "Id";

        protected MongoEntityDataAccess(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 建立索引（使用 MongoDB.Entities 的 API）
        /// </summary>
        protected void EnsureIndexCreated(string searchKey)
        {
            // 因為Mongodb.Entity只支援使用TDocument上的變數名稱做索引
            // 當需要從外部設定時 就用回MongoDB.Driver去設定索引
            if (!_bIndexCreated)
            {
                _bIndexCreated      = true;
                _searchKey          = searchKey;

                var indexKeys       = Builders<TDocument>.IndexKeys.Ascending(searchKey); // searchKey 是 string
                var indexOptions    = new CreateIndexOptions { Unique = true };
                var indexModel      = new CreateIndexModel<TDocument>(indexKeys, indexOptions);

                DB.Collection<TDocument>().Indexes.CreateOne(indexModel);
            }
        }

        protected virtual Task<TEntity> MapToEntity(TDocument doc, IMapper mapper)
        {
            // 使用 AutoMapper 進行映射
            return Task.FromResult(mapper.Map<TEntity>(doc));
        }

        protected virtual TDocument MapToDocument(TEntity entity, IMapper mapper)
        {
            // 使用 AutoMapper 進行映射
            return mapper.Map<TDocument>(entity);
        }

        public virtual async Task<TEntity?> InsertAsync(TEntity entity)
        {
            var doc= MapToDocument(entity, _mapper);
            await doc.SaveAsync();
            return MapToEntity(doc, _mapper).Result; 
        }

        public virtual async Task<TEntity?> QueryAsync(string key)
        {
            // 建立 Key 過濾條件
            var keyFilter       = Builders<TDocument>.Filter.Eq(_searchKey, key);

            // 執行查詢
            var doc = await DB.Find<TDocument>()
                                    .Match(keyFilter)
                                    .ExecuteFirstAsync();

            if (doc == null)
            {
                return null;
            }

            return await MapToEntity(doc, _mapper);
        }

        public virtual async Task<List<TEntity>?> QueryAllAsync()
        {
            List<TDocument> docs    = await DB.Find<TDocument>()
                                    .Match(_ => true)
                                    .ExecuteAsync();

            // 非同步轉換所有文件 → Entity
            var entities            = await Task.WhenAll(docs.Select(doc => MapToEntity(doc, _mapper)));
            return entities.ToList();
        }

        public virtual async Task<List<TEntity>?> QueryAsync(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return null;
            }
            // 建立 Key 過濾條件
            var keyFilter       = Builders<TDocument>.Filter.In(_searchKey, keys);


            // 執行查詢
            var docs = await DB.Find<TDocument>()
                                    .Match(keyFilter)
                                    .ExecuteAsync();

            // 非同步轉換所有文件 → Entity
            var entities    = await Task.WhenAll(docs.Select(doc => MapToEntity(doc, _mapper)));
            return entities.ToList();
        }

        public virtual async Task<List<TEntity>?> QueryByTimeAsync(DateTime? startTime, DateTime? endTime)
        {
            Find<TDocument, TDocument> query = DB.Find<TDocument>();

            if (startTime.HasValue)
            {
                query = query.Match(d => d.CreatedAt >= startTime.Value);
            }

            if (endTime.HasValue)
            {
                query = query.Match(d => d.CreatedAt <= endTime.Value);
            }

            var docs        = await query.ExecuteAsync();
            // 非同步轉換所有文件 → Entity
            var entities    = await Task.WhenAll(docs.Select(doc => MapToEntity(doc, _mapper)));
            return entities.ToList();
        }

        public virtual async Task<bool> UpdateAsync(string key, TEntity entity, List<string>? noUpdateList = null)
        {
            var doc             = MapToDocument(entity, _mapper);
            var excludedFields  = new HashSet<string>(
                new[] { "_id", "CreatedAt", _searchKey }
                .Concat(noUpdateList ?? Enumerable.Empty<string>())
            );


            doc.Id              = ObjectId.GenerateNewId().ToString();// 隨便產生 因為不會進入資料庫 只是要能順利ToBsonDocument
            var bsonDoc         = doc.ToBsonDocument();
            var updateDict      = bsonDoc
                .Where(kv => !excludedFields.Contains(kv.Name))
                .ToDictionary(kv => kv.Name, kv => kv.Value);

            if (!updateDict.Any())
            {
                return false;
            }

            var update = DB.Update<TDocument>().Match(d => d.Eq(_searchKey, key));

            foreach (var kv in updateDict)
            {
                update = update.Modify(b => b.Set(kv.Key, kv.Value));
            }

            var result = await update.ExecuteAsync();

            return result.ModifiedCount > 0;
        }


        public virtual async Task<bool> DeleteAsync(string key)
        {
            var deletedResult = await DB.DeleteAsync<TDocument>(d => d.Eq(_searchKey, key));
            return deletedResult.DeletedCount > 0;
        }

        public virtual async Task<bool> ExistsAsync(string key)
        {
            var count = await DB.CountAsync<TDocument>(d => d.Eq(_searchKey, key));
            return count > 0;
        }

        public virtual async Task<bool> ExistsAsync(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return false;
            }

            var count = await DB.CountAsync<TDocument>(d => d.In(_searchKey, keys));
            return count > 0;
        }

        public virtual async Task<TEntity?> FindLastAsync()
        {
            var doc = await DB.Find<TDocument>()
                              .Sort(d => d.Descending(x => x.UpdatedAt))
                              .ExecuteFirstAsync();
            if (doc == null)
            {
                return null;
            }

            return await MapToEntity(doc, _mapper);
        }
    }
}
