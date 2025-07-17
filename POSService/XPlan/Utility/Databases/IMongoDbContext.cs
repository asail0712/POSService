using MongoDB.Driver;

namespace XPlan.Utility.Databases
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
