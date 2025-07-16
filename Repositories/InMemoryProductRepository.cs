using Shopping_Cart.Interfaces;
using Shopping_Cart.Models;

namespace Shopping_Cart.Repositories
{
    public class InMemoryProductRepository: IProductRepository
    {
        private static readonly List<Product> _products = new()
        {
            new() { Id = 1, Name = "Laptop", Price = 1200, Description = "Laptop poderosa", ImageUrl = "/img/laptop.jpg" },
            new() { Id = 2, Name = "Mouse", Price = 25, Description = "Mouse óptico", ImageUrl = "/img/mouse.jpg" },
        };

        public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult(_products.AsEnumerable());
        public Task<Product?> GetByIdAsync(int id) => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
    }
}
