using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CartService.Data.ViewModel;
using CartService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public class AddItemRequest
        {
            public Guid ItemId { get; set; }

            public int Quantity { get; set; } = 1;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            if (!TryGetUserId(out var userId)) return Unauthorized("Token does not contain a valid user id claim.");

            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null) return NotFound("Cart not found.");

            return Ok(CartViewModel.FromModel(cart));
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddItemRequest request)
        {
            if (request == null || request.ItemId == Guid.Empty) return BadRequest("Invalid request");
            if (!TryGetUserId(out var userId)) return Unauthorized("Token does not contain a valid user id claim.");

            var ok = await _cartService.AddItemToUserCartAsync(userId, request.ItemId, Math.Max(1, request.Quantity));
            if (!ok) return NotFound("Item or user not found.");

            return Ok();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            if (itemId == Guid.Empty) return BadRequest("Invalid request");
            if (!TryGetUserId(out var userId)) return Unauthorized("Token does not contain a valid user id claim.");

            var ok = await _cartService.RemoveItemFromUserCartAsync(userId, itemId);
            if (!ok) return NotFound("Item or cart not found.");

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCart()
        {
            if (!TryGetUserId(out var userId)) return Unauthorized("Token does not contain a valid user id claim.");

            var ok = await _cartService.DeleteCartAsync(userId);
            if (!ok) return NotFound("Cart not found.");

            return Ok();
        }

        // The token's own claim is the only source of identity here — never accept
        // a caller-supplied user id, or any authenticated user could act on someone else's cart.
        private bool TryGetUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            return idClaim != null && Guid.TryParse(idClaim.Value, out userId);
        }
    }
}
