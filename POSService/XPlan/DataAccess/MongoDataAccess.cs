using MongoDB.Bson;
using MongoDB.Driver;

using XPlan.Utility.Databases;
using XPlan.Entities;

namespace XPlan.DataAccess
{
    public abstract class MongoDataAccess<TEntity>// : IDataAccess<TEntity> 
        where TEntity : IDBEntity
    {
        private readonly IMongoCollection<TEntity> _collection;
        private static bool _bIndexCreated  = false;
        private static string _searchKey    = "Id";
        public MongoDataAccess(IMongoDbContext dbContext, IDBSetting dbSettings)
        {
            this._collection    = dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        protected void EnsureIndexCreated(string searchKey)
        {
            if (!_bIndexCreated)
            {
                _bIndexCreated      = true;
                _searchKey          = searchKey;

                var indexKeys       = Builders<TEntity>.IndexKeys.Ascending(searchKey);
                var indexOptions    = new CreateIndexOptions { Unique = true };
                var indexModel      = new CreateIndexModel<TEntity>(indexKeys, indexOptions);
                _collection.Indexes.CreateOne(indexModel);                
            }
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);

            return entity;
        }

        public virtual async Task<List<TEntity>> QueryAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<TEntity> QueryAsync(string key)
        {
            var filter = Builders<TEntity>.Filter.Eq(_searchKey, key);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>> QueryAsync(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return new List<TEntity>();
            }

            var filter = Builders<TEntity>.Filter.In(_searchKey, keys);

            return await _collection.Find(filter).ToListAsync();
        }

        public virtual async Task<List<TEntity>> QueryByTimeAsync(DateTime? startTime, DateTime? endTime)
        {
            // 如果 startTime 和 endTime 都沒設定，直接回傳所有資料
            if (startTime == null && endTime == null)
            {
                return await _collection.Find(_ => true).ToListAsync();
            }

            // 如果兩個時間都有，並且 startTime > endTime，就交換
            if (startTime != null && endTime != null && startTime > endTime)
            {
                return new List<TEntity>();
            }

            // 建立過濾條件
            var builder                         = Builders<TEntity>.Filter;
            FilterDefinition<TEntity> filter    = builder.Empty;

            if (startTime != null)
            {
                filter = builder.And(filter, builder.Gte(e => e.CreatedAt, startTime.Value));
            }
            if (endTime != null)
            {
                filter = builder.And(filter, builder.Lte(e => e.CreatedAt, endTime.Value));
            }

            return await _collection.Find(filter).ToListAsync();
        }

        public virtual async Task<bool> UpdateAsync(string key, TEntity entity, List<string>? noUpdateList = null)
        {            
            var filter          = Builders<TEntity>.Filter.Eq(_searchKey, key);
            var bsonDoc         = entity.ToBsonDocument();              // 將 Entity 轉成 BsonDocument

            // 欄位黑名單：_id、CreatedAt、noUpdateList
            var excludedFields = new HashSet<string>(
                new[] { "_id", "CreatedAt"}
                .Concat(noUpdateList ?? Enumerable.Empty<string>())
            );

            // 過濾要更新的欄位
            var updateFields = bsonDoc
                .Where(kv => !excludedFields.Contains(kv.Name))
                .ToDictionary(kv => kv.Name, kv => kv.Value);

            if (!updateFields.Any())
            {
                // 沒有要更新的欄位
                return false;
            }

            var update  = new BsonDocument("$set", new BsonDocument(updateFields));    // 使用 $set 更新所有欄位
            var result  = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        public virtual async Task<bool> DeleteAsync(string key)
        {
            var filter  = Builders<TEntity>.Filter.Eq(_searchKey, key);
            var result  = await _collection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        public virtual async Task<bool> ExistsAsync(string key)
        {
            var filter  = Builders<TEntity>.Filter.Eq(_searchKey, key);
            var count   = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public virtual async Task<bool> ExistsAsync(List<string> key)
        {
            if (key == null || key.Count == 0)
            {
                return false;
            }
            var filter = Builders<TEntity>.Filter.In(_searchKey, key);
            var count  = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public virtual async Task<TEntity> FindLastAsync()
        {
            var sort = Builders<TEntity>.Sort.Descending(nameof(IDBEntity.UpdatedAt));

            return await _collection
                .Find(_ => true)
                .Sort(sort)
                .Limit(1)
                .FirstOrDefaultAsync();
        }
    }
}
