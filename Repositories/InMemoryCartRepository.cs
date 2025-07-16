using Shopping_Cart.Interfaces;
using Shopping_Cart.Models;

namespace Shopping_Cart.Repositories
{
    public class InMemoryCartRepository: ICartRepository
    {
        private static readonly List<CartItem> _cart = new();

        public Task<IEnumerable<CartItem>> GetCartAsync() => Task.FromResult(_cart.AsEnumerable());

        public Task AddToCartAsync(CartItem item)
        {
            var existing = _cart.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existing != null)
                existing.Quantity += item.Quantity;
            else
                _cart.Add(item);

            return Task.CompletedTask;
        }


        public Task UpdateQuantityAsync(int productId, int delta)
        {
            var item = _cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                item.Quantity += delta;
                if (item.Quantity <= 0)
                {
                    _cart.Remove(item);
                }
            }
            return Task.CompletedTask;
        }

        public Task RemoveFromCartAsync(int productId)
        {
            _cart.RemoveAll(c => c.ProductId == productId);
            return Task.CompletedTask;
        }

        public Task ClearCartAsync()
        {
            _cart.Clear();
            return Task.CompletedTask;
        }
    }
}
