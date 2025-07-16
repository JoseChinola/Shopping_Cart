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
        public async Task<IActionResult> GetCart()
        {
            var items = await _cartRepository.GetCartAsync();

            var totalItems = items.Sum(i => i.Quantity);
            var totalPrice = items.Sum(i => i.Quantity * i.Product.Price);

            return Ok(new
            {
                items,
                totalItems,
                totalPrice
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(CartItemDto cartDto)
        {
            var product = await _productRepository.GetByIdAsync(cartDto.ProductId);
            if (product == null)
            {
                return NotFound($"Product with ID {cartDto.ProductId} not found.");
            }

            await _cartRepository.AddToCartAsync(new CartItem
            {
                ProductId = cartDto.ProductId,
                Quantity = cartDto.Quantity,
                Product = product
            });

            return Ok("Item added to cart successfully.");
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDto UpdatDto)
        {
           var product = await _productRepository.GetByIdAsync(UpdatDto.ProductId);
            if (product == null)
            {
                return NotFound($"Product with ID {UpdatDto.ProductId} not found.");
            }
            var cartItems = await _cartRepository.GetCartAsync();
            if (!cartItems.Any(c => c.ProductId == UpdatDto.ProductId))
            {
                return NotFound($"Item with Product ID {UpdatDto.ProductId} not found in cart.");
            }
            await _cartRepository.UpdateQuantityAsync(UpdatDto.ProductId, UpdatDto.Delta);
            return Ok("Item quantity updated successfully.");
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartItems = await _cartRepository.GetCartAsync();
            if (!cartItems.Any(c => c.ProductId == productId))
            {
                return NotFound($"Item with Product ID {productId} not found in cart.");
            }
            await _cartRepository.RemoveFromCartAsync(productId);
            return Ok("Item removed from cart successfully.");
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cartItems = await _cartRepository.GetCartAsync();
            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty.");
            }          
            await _cartRepository.ClearCartAsync();
            return Ok("Checkout successful. Your order has been placed.");
        }
    }
}
