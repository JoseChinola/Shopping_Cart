using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
