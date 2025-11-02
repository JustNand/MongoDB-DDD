using DDDExample.Domain.Entities;
using DDDExample.Domain.Repositories;
using MongoDB.Driver;

namespace DDDExample.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Product>("Products");
        }

        public async Task AddAsync(Product product)
        {
            await _collection.InsertOneAsync(product);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(p => p.Id.ToString() == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _collection.Find(p => p.Id.ToString() == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _collection.ReplaceOneAsync(p => p.Id == product.Id, product);
        }
    }
}