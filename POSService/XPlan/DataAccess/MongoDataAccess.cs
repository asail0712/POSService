using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XPlan.Utility.Databases;
using XPlan.Entity;

namespace XPlan.DataAccess
{
    public abstract class MongoDataAccess<TEntity> : IDataAccess<TEntity> where TEntity : EntityBase
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoDataAccess(IMongoClient mongoClient, IDBSetting dbSettings)
        {
            var database        = mongoClient.GetDatabase(dbSettings.DatabaseName);
            this._collection    = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(entity);
        }

        public virtual async Task<List<TEntity>?> QueryAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual async Task<TEntity?> QueryAsync(string key)
        {
            return await _collection.Find(e => e.SearchKey == key).FirstOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>?> QueryAsync(List<string> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return new List<TEntity>();
            }

            var filter = Builders<TEntity>.Filter.In(e => e.SearchKey, keys);

            return await _collection.Find(filter).ToListAsync();
        }

        public virtual async Task<List<TEntity>?> QueryByTimeAsync(DateTime? startTime, DateTime? endTime)
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
            entity.UpdatedAt    = DateTime.UtcNow;
            var filter          = Builders<TEntity>.Filter.Eq(x => x.SearchKey, key);
            var bsonDoc         = entity.ToBsonDocument();              // 將 Entity 轉成 BsonDocument

            // 欄位黑名單：_id、CreatedAt、noUpdateList
            var excludedFields = new HashSet<string>(
                new[] { "_id", "CreatedAt" }
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
            var result = await _collection.DeleteOneAsync(e => e.SearchKey == key);

            return result.DeletedCount > 0;
        }

        public virtual async Task<bool> ExistsAsync(string key)
        {
            var count = await _collection.CountDocumentsAsync(e => e.SearchKey == key);
            return count > 0;
        }

        public virtual async Task<bool> ExistsAsync(List<string> key)
        {
            if (key == null || key.Count == 0)
            {
                return false;
            }
            var filter = Builders<TEntity>.Filter.In(e => e.SearchKey, key);
            var count  = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
