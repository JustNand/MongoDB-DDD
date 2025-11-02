using DDDExample.Domain.Common;
using MongoDB.Driver;

namespace DDDExample.Infrastructure.Persistence
{
    public class MongoDBContext<T> where T : Entity
    {
        private readonly IMongoCollection<T> _collection;

        public MongoDBContext(IMongoDatabase database, string collectionName)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("El nombre de la colección no puede estar vacío", nameof(collectionName));

            _collection = database.GetCollection<T>(collectionName);
        }

        public IMongoCollection<T> Collection => _collection;
    }
}