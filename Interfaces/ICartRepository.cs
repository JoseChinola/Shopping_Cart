using Shopping_Cart.Models;

namespace Shopping_Cart.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetCartAsync(int userId);
        Task AddToCartAsync(CartItem item);
        Task UpdateQuantityAsync(int userId, int productId, int delta);
        Task RemoveFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
