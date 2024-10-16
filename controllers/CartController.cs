using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shopdotcobackend.Models;
using Newtonsoft.Json;
using shopdotcobackend.Services;

namespace shopdotcobackend.controllers
{
    
        [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly SupabaseService _supabaseService;

        public CartController(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        // GET: api/cart/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartItemsByUserId(string userId)
        {
            var cartItems = await _supabaseService.GetCartItemsByUserId(userId);
            return Ok(cartItems);
        }

        // POST: api/cart
        [HttpPost]
        public async Task<ActionResult> AddToCart([FromBody] Cart cart)
        {
            var addedCartItem = await _supabaseService.AddToCart(cart);
            return CreatedAtAction(nameof(GetCartItemsByUserId), new { user_id = addedCartItem?.user_id }, addedCartItem);
        }

        // PUT: api/cart/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCartItem(int id, [FromBody] int quantity)
        {
            var updatedCartItem = await _supabaseService.UpdateCartItem(id, quantity);
            if (updatedCartItem == null)
            {
                return NotFound($"Cart item with ID {id} not found.");
            }
            return Ok(updatedCartItem);
        }

        // DELETE: api/cart/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            var deleted = await _supabaseService.DeleteCartItem(id);
            if (!deleted)
            {
                return NotFound($"Cart item with ID {id} not found.");
            }
            return Ok("Cart item deleted successfully.");
        }
    }
}