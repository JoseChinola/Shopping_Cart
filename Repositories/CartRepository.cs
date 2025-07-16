using Microsoft.EntityFrameworkCore;
using Shopping_Cart.Data;
using Shopping_Cart.Interfaces;
using Shopping_Cart.Models;

namespace Shopping_Cart.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartAsync()
        {
            return await _context.CartItems.Include(c => c.Product).ToListAsync();
        }

        public async Task AddToCartAsync(CartItem item)
        {
            var existing = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == item.ProductId);
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                await _context.CartItems.AddAsync(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(int productId, int delta)
        {
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (item != null)
            {
                item.Quantity += delta;
                if (item.Quantity <= 0)
                {
                    _context.CartItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCartAsync(int productId)
        {
            var allItems = await _context.CartItems.ToListAsync();
            _context.CartItems.RemoveRange(allItems);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync()
        {
            var items = await _context.CartItems.ToListAsync();
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
