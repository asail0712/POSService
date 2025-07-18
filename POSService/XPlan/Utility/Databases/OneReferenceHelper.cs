using MongoDB.Entities;

namespace XPlan.Utility.Databases
{
    internal static class OneReferenceHelper
    {
        public static string? GetId<TDocument>(One<TDocument> one)
            where TDocument : Entity
        {
            return one?.ID;
        }

        public static One<TDocument>? ToOne<TDocument>(string? id)
            where TDocument : Entity
        {
            return string.IsNullOrEmpty(id) ? null : new One<TDocument>(id);
        }

        public static async Task<TDocument?> LoadEntityAsync<TDocument>(One<TDocument>? one)
            where TDocument : Entity
        {
            return one == null ? null : await one.ToEntityAsync();
        }

        public static async Task<List<TDocument>?> LoadEntitysAsync<TDocument>(List<One<TDocument>>? ones)
            where TDocument : Entity
        {
            return ones == null ? null : (await Task.WhenAll(ones.Select(item => item.ToEntityAsync()))).ToList();
        }

        public static One<TDocument>? ToReference<TDocument>(TDocument? entity)
            where TDocument : Entity
        {
            return entity == null ? null : entity.ToReference();
        }
    }
}
