using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities;
using System.Linq.Expressions;
using XPlan.Entities;
using XPlan.Utility.Databases;

namespace XPlan.DataAccess
{
    public abstract class MongoDataAccess<TEntity>// : IDataAccess<TEntity> 
        where TEntity : IDBEntity
    {
        private readonly IMongoCollection<TEntity> _collection;
        private static bool _bIndexCreated          = false;
        private static string _searchFieldName      = "Id";
        private List<string> _noUpdateList;

        public MongoDataAccess(IMongoDbContext dbContext)
        {
            this._collection    = dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
            this._noUpdateList  = new List<string>();
        }

        protected void AddNoUpdateKey(string noUpdateKey)
        {
            _noUpdateList.Add(noUpdateKey);
            _noUpdateList.Distinct();
        }

        protected void EnsureIndexCreated(string searchFieldName)
        {
            if (!_bIndexCreated)
            {
                _bIndexCreated      = true;
                _searchFieldName    = searchFieldName;

                var indexKeys       = Builders<TEntity>.IndexKeys.Ascending(searchFieldName);
                var indexOptions    = new CreateIndexOptions { Unique = true };
                var indexModel      = new CreateIndexModel<TEntity>(indexKeys, indexOptions);

                _collection.Indexes.CreateOne(indexModel);                
            }
        }

        public virtual async Task<TEntity?> InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);

            return entity;
        }

        public virtual async Task<List<TEntity>?> QueryAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();         
        }

        public virtual async Task<TEntity?> QueryAsync(string key)
        {
            var baseFilter = Builders<TEntity>.Filter.Eq(_searchFieldName, key);

            return await _collection.Find(baseFilter).FirstOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>?> QueryAsync(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return null;
            }

            var baseFilter      = Builders<TEntity>.Filter.In(_searchFieldName, keys);

            return await _collection.Find(baseFilter).ToListAsync();
        }

        public virtual async Task<List<TEntity>?> QueryAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _collection.Find(predicate)
                                       .ToListAsync();

            return entities;
        }

        public virtual async Task<bool> UpdateAsync(string key, TEntity entity)
        {
            var filter          = Builders<TEntity>.Filter.Eq(_searchFieldName, key);
            var bsonDoc         = entity.ToBsonDocument();              // 將 Entity 轉成 BsonDocument

            // 欄位黑名單：_id、CreatedAt、noUpdateList
            var excludedFields  = new HashSet<string>(
                            new[] { "_id", "CreatedAt", _searchFieldName }
                            .Concat(_noUpdateList ?? Enumerable.Empty<string>())
                        ).Distinct();

            // 過濾要更新的欄位
            var updateFields    = bsonDoc
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
            var filter  = Builders<TEntity>.Filter.Eq(_searchFieldName, key);
            var result  = await _collection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        public virtual async Task<bool> ExistsAsync(string key)
        {
            var filter  = Builders<TEntity>.Filter.Eq(_searchFieldName, key);
            var count   = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public virtual async Task<bool> ExistsAsync(List<string> key)
        {
            if (key == null || key.Count == 0)
            {
                return false;
            }

            var filter = Builders<TEntity>.Filter.In(_searchFieldName, key);
            var count  = await _collection.CountDocumentsAsync(filter);

            return count > 0;
        }

        public virtual async Task<TEntity?> FindLastAsync()
        {
            var sort = Builders<TEntity>.Sort.Descending(nameof(IDBEntity.UpdatedAt));

            return await _collection
                            .Find(_ => true)
                            .Sort(sort)
                            .Limit(1)
                            .FirstOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>?> QueryAndUpdateAsync(Expression<Func<TEntity, bool>> predicate, Action<TEntity> updateAction)
        {
            // 查詢所有符合條件的 Entity
            var entities = await _collection.Find(predicate).ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return new List<TEntity>();
            }

            // 執行更新邏輯並逐一更新資料庫
            foreach (var entity in entities)
            {
                updateAction(entity);

                var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id); // 假設有 Id 屬性
                await _collection.ReplaceOneAsync(filter, entity);
            }

            return entities;
        }
    }
}
