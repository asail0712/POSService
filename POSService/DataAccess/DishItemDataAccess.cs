using AutoMapper;
using Common.DTO.Dish;
using DataAccess.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.DataAccess;
using XPlan.Utility.Databases;

namespace DataAccess
{
    public class DishItemDataAccess : MongoEntityDataAccess<DishItemEntity, DishItemDocument>, IDishItemDataAccess
    {
        public DishItemDataAccess(IMapper mapper)
            : base(mapper)
        {
        }

        public async Task<int> ReduceStock(string key, int numOfReduce)
        {
            var filter = Builders<DishItemDocument>.Filter.And(
                Builders<DishItemDocument>.Filter.Eq(p => p.Id, key),
                Builders<DishItemDocument>.Filter.Gte(p => p.Stock, numOfReduce) // 確保庫存足夠
            );

            var update = Builders<DishItemDocument>.Update.Inc(p => p.Stock, -numOfReduce);

            var options = new FindOneAndUpdateOptions<DishItemDocument>
            {
                ReturnDocument = ReturnDocument.After // 回傳更新後的 document
            };

            var updatedDoc = await DB.Collection<DishItemDocument>()
                                     .FindOneAndUpdateAsync(filter, update, options);

            return updatedDoc.Stock;
        }
    }
}
