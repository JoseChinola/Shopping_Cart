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
                return BadRequest(new { message = "Invalid cart item data." });
            }

            // Check if the product exists
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                return NotFound(new {message = $"Product with ID {item.ProductId} not found." });
            }

            // Check if the product is available in sufficient quantity
            await _cartRepository.AddToCartAsync(new CartItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Product = product,
                userId = item.UserId
            });


            return Ok(new
            {
                message = "Item added to cart successfully.",
                success = true

            });           
        }

        [HttpPut("quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDto dto)
        {

            // Validate the input
            if (dto == null || dto.ProductId <= 0 || dto.Delta == 0)
            {
                return BadRequest(new { message = "Invalid update quantity data." });
            }

        
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {dto.ProductId} not found." });
            }

            // Validate the quantity update
            var cartItems = await _cartRepository.GetCartAsync(dto.UserId);
            if (!cartItems.Any(c => c.ProductId == dto.ProductId))
            {
                return NotFound(new {message =$"Item with Product ID {dto.ProductId} not found in cart."});
            }

            await _cartRepository.UpdateQuantityAsync(dto.UserId, dto.ProductId, dto.Delta);

            return Ok(new
            {
                message = "Item quantity updated successfully.",
                success = true

            });
       
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveItem(int userId, int productId)
        {
            // Validate the input
            var cartItems = await _cartRepository.GetCartAsync(userId);
            if (!cartItems.Any(c => c.ProductId == productId))
            {
                return NotFound(new { message = $"Item with Product ID {productId} not found in cart." });
            }

            // Check if the product exists
            await _cartRepository.RemoveFromCartAsync(userId, productId);

            return Ok(new
            {
               message = "Item removed from cart successfully.",
               success = true
                               
            });
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromQuery] int userId)
        {
            // Validate the user ID
            var cartItems = await _cartRepository.GetCartAsync(userId);
            if (!cartItems.Any())
            {
                return BadRequest(new { message = "Cart is empty.", success = false, error = true });
            }

            await _cartRepository.ClearCartAsync(userId);
            return Ok(new { message = "Payment successful", success = true, error = false });
        }
    }
}
