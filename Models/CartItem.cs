using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Cart.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
