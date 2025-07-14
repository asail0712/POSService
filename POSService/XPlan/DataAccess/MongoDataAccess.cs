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
using XPlan.Interface;

namespace XPlan.DataAccess
{
    public abstract class MongoDataAccess<TEntity> : IDataAccess<TEntity> where TEntity : class, IEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoDataAccess(IMongoClient mongoClient, IDBSetting dbSettings)
        {
            var database        = mongoClient.GetDatabase(dbSettings.DatabaseName);
            this._collection    = database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task InsertAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<TEntity?>?> QueryAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<TEntity?> QueryByIdAsync(string id)
        {
            return await _collection.Find(e => e.Id == new ObjectId(id)).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string id, TEntity entity)
        {
            var result = await _collection.ReplaceOneAsync(e => e.Id == new ObjectId(id), entity);

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(e => e.Id == new ObjectId(id));

            return result.DeletedCount > 0;
        }
    }
}
