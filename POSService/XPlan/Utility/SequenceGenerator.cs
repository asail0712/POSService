using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Databases;

namespace XPlan.Utility
{
    public interface ISequenceGenerator
    {
        Task<string> GetNextSequenceAsync(string name, long startValue = 1, int digits = 3);
    }

    public class SequenceGenerator : ISequenceGenerator
    {
        private readonly IMongoCollection<BsonDocument> _counterCollection;

        public SequenceGenerator(IMongoDbContext dbContext)
        {
            // 專門存放所有流水號的集合
            _counterCollection = dbContext.GetCollection<BsonDocument>("counters");
        }

        public async Task<string> GetNextSequenceAsync(string name, long startValue = 1, int digits = 3)
        {
            var filter  = Builders<BsonDocument>.Filter.Eq("_id", name);
            var update  = Builders<BsonDocument>.Update.Inc("seq", 1);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument  = ReturnDocument.After,
                IsUpsert        = true
            };

            var result = await _counterCollection.FindOneAndUpdateAsync(filter, update, options);

            if (!result.Contains("seq"))
            {
                // 如果新建，初始化 seq
                var initUpdate = Builders<BsonDocument>.Update.Set("seq", startValue);
                result = await _counterCollection.FindOneAndUpdateAsync(filter, initUpdate, options);
            }

            long sequenceNumber = result["seq"].AsInt32;

            // 根據 digits 格式化
            return sequenceNumber.ToString($"D{digits}");
        }
    }
}
