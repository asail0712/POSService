using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;

using XPlan.Entities;

namespace XPlan.DataAccess
{
    public abstract class MongoEntityDataAccess<TEntity, TDocument>
        where TEntity : class, new()
        where TDocument : Entity, Entities.IEntity, new() // 👈 注意繼承 Entity
    {
        private static bool _bIndexCreated  = false;
        private static string _searchKey    = "Id";

        protected MongoEntityDataAccess()
        {

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

        protected abstract TEntity MapToEntity(TDocument doc);
        protected abstract TDocument MapToDocument(TEntity entity);

        public virtual async Task InsertAsync(TEntity entity)
        {
            var doc = MapToDocument(entity);
            SetTimestamps(doc, isInsert: true);
            await doc.SaveAsync();
        }

        public virtual async Task<TEntity?> QueryAsync(string key)
        {
            var doc = await DB.Find<TDocument>()
                              .Match(d => d.Eq(_searchKey, key))
                              .ExecuteFirstAsync();
            return doc == null ? null : MapToEntity(doc);
        }

        public virtual async Task<List<TEntity>> QueryAllAsync()
        {
            var docs = await DB.Find<TDocument>()
                               .Match(_ => true)
                               .ExecuteAsync();
            return docs.Select(MapToEntity).ToList();
        }

        public virtual async Task<List<TEntity>> QueryAsync(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
                return new List<TEntity>();

            var docs = await DB.Find<TDocument>()
                               .Match(d => d.Eq(_searchKey, keys))
                               .ExecuteAsync();
            return docs.Select(MapToEntity).ToList();
        }

        public virtual async Task<List<TEntity>> QueryByTimeAsync(DateTime? startTime, DateTime? endTime)
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

            var docs = await query.ExecuteAsync();
            return docs.Select(MapToEntity).ToList();
        }

        public virtual async Task<bool> UpdateAsync(string key, TEntity entity, List<string>? noUpdateList = null)
        {
            var doc = MapToDocument(entity);
            SetTimestamps(doc, isInsert: false);

            var excludedFields = new HashSet<string>(
                new[] { "_id", "CreatedAt" }
                .Concat(noUpdateList ?? Enumerable.Empty<string>())
            );

            var bsonDoc = doc.ToBsonDocument();
            var updateDict = bsonDoc
                .Where(kv => !excludedFields.Contains(kv.Name))
                .ToDictionary(kv => kv.Name, kv => kv.Value);

            if (!updateDict.Any())
                return false;

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
                return false;

            var count = await DB.CountAsync<TDocument>(d => d.In(_searchKey, keys));
            return count > 0;
        }

        public virtual async Task<TEntity?> FindLastAsync()
        {
            var doc = await DB.Find<TDocument>()
                              .Sort(d => d.Descending(x => x.UpdatedAt))
                              .ExecuteFirstAsync();
            return doc == null ? null : MapToEntity(doc);
        }

        private void SetTimestamps(TDocument doc, bool isInsert)
        {
            var now = DateTime.UtcNow;

            if (isInsert && doc.GetType().GetProperty("CreatedAt") != null)
            {
                doc.GetType().GetProperty("CreatedAt")?.SetValue(doc, now);
            }

            if (doc.GetType().GetProperty("UpdatedAt") != null)
            {
                doc.GetType().GetProperty("UpdatedAt")?.SetValue(doc, now);
            }
        }
    }
}
