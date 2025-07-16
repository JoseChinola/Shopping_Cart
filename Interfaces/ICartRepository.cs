using Shopping_Cart.Models;

namespace Shopping_Cart.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetCartAsync();
        Task AddToCartAsync(CartItem item);
        Task RemoveFromCartAsync(int productId);
        Task ClearCartAsync();
        Task UpdateQuantityAsync(int productId, int delta);
    }
}
