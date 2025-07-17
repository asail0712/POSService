using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Utility.Databases
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
