using Microsoft.EntityFrameworkCore;
using Shopping_Cart.Models;

namespace Shopping_Cart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


    }
}
