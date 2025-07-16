using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping_Cart.DTOs;
using Shopping_Cart.Interfaces;
using Shopping_Cart.Models;

namespace Shopping_Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;



        public CartController(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] int userId)
        {
            var items = await _cartRepository.GetCartAsync(userId);

            

           

            if (!items.Any())
            {
                return Ok(new
                {
                    items = new List<CartItem>(),
                    totalItems = 0,
                    totalPrice = 0.0
                });
            }

            var mappedItems = items.Select(i => new
            {
                i.Id,
                i.Quantity,
                product = new
                {
                    i.Product?.Id,
                    i.Product?.Name,
                    i.Product?.Price,
                    i.Product?.ImageUrl
                },
                User = new
                {
                    i.User.Id,
                    i.User.Name,
                    i.User.Username
                }
            });


            var totalItems = items.Sum(i => i.Quantity);
            var totalPrice = items.Sum(i => i.Quantity * (i.Product?.Price ?? 0));

            return Ok(new
            {
                message= "Cart retrieved successfully",
                items = mappedItems,
                totalItems,
                totalPrice
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(CartItemDto item)
        {
            // Validate the input
            if (item == null || item.ProductId <= 0 || item.Quantity <= 0)
            {
                return BadRequest("Invalid cart item data.");
            }

            // Check if the product exists
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                return NotFound($"Product with ID {item.ProductId} not found.");
            }

            // Check if the product is available in sufficient quantity
            await _cartRepository.AddToCartAsync(new CartItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Product = product,
                userId = item.UserId
            });

            return Ok("Item added to cart successfully.");
        }

        [HttpPut("quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDto dto)
        {

            // Validate the input
            if (dto == null || dto.ProductId <= 0 || dto.Delta == 0)
            {
                return BadRequest("Invalid update quantity data.");
            }

        
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                return NotFound($"Product with ID {dto.ProductId} not found.");
            }

            // Validate the quantity update
            var cartItems = await _cartRepository.GetCartAsync(dto.UserId);
            if (!cartItems.Any(c => c.ProductId == dto.ProductId))
            {
                return NotFound($"Item with Product ID {dto.ProductId} not found in cart.");
            }

            await _cartRepository.UpdateQuantityAsync(dto.UserId, dto.ProductId, dto.Delta);

            return Ok("Item quantity updated successfully.");
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveItem(int userId, int productId)
        {
            // Validate the input
            var cartItems = await _cartRepository.GetCartAsync(userId);
            if (!cartItems.Any(c => c.ProductId == productId))
            {
                return NotFound($"Item with Product ID {productId} not found in cart.");
            }

            // Check if the product exists
            await _cartRepository.RemoveFromCartAsync(userId, productId);

            return Ok("Item removed from cart successfully.");
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] int userId)
        {
            // Validate the user ID
            var cartItems = await _cartRepository.GetCartAsync(userId);
            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty.");
            }
            
            await _cartRepository.ClearCartAsync(userId);
            return Ok("Checkout successful. Your order has been placed.");
        }
    }
}
