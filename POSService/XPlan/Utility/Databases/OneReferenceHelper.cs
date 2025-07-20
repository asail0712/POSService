using MongoDB.Entities;

namespace XPlan.Utility.Databases
{
    public static class OneReferenceHelper
    {
        public static string? GetId<TDocument>(One<TDocument> one)
            where TDocument : IEntity
        {
            return one?.ID;
        }

        public static One<TDocument>? ToOne<TDocument>(string? id)
            where TDocument : IEntity
        {
            return string.IsNullOrEmpty(id) ? null : new One<TDocument>(id);
        }

        public static async Task<TDocument?> LoadEntityAsync<TDocument>(One<TDocument>? one)
            where TDocument : IEntity
        {
            return one == null ? default(TDocument) : await one.ToEntityAsync();
        }

        public static async Task<List<TDocument>?> LoadEntitysAsync<TDocument>(List<One<TDocument>>? ones)
            where TDocument : IEntity
        {
            return ones == null ? null : (await Task.WhenAll(ones.Select(item => item.ToEntityAsync()))).ToList();
        }

        public static One<TDocument>? ToRefer<TDocument>(TDocument? entity)
            where TDocument : IEntity
        {
            return entity == null ? null : entity.ToReference();
        }

        public static List<One<TDocument>>? ToRefer<TDocument>(List<TDocument>? docs)
            where TDocument : IEntity
        {
            if(docs == null)
            {
                return null;
            }

            return docs.Select(doc => doc.ToReference()).ToList();
        }
    }
}
