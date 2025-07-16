namespace Shopping_Cart.DTOs
{
    public class UpdateQuantityDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Delta { get; set; }
        
    }
}
