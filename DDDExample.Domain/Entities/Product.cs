using DDDExample.Domain.Common;

namespace DDDExample.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        public Product(string name, string description, decimal price, int stock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }

        // Método para actualizar el stock
        public void UpdateStock(int newStock)
        {
            Stock = newStock;
        }
    }
}