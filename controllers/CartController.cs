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
        // public async Task<ActionResult<IEnumerable<Cart>>> GetCartItemsByUserId(string userId)
        // {
        //     var cartItems = await _supabaseService.GetCartItemsByUserId(userId);
        //     return Ok(cartItems);
        // }

        public async Task<ActionResult<IEnumerable<CartItemWithProduct>>> GetCartItemsByUserId(string userId)
{
    var cartItemsWithProducts = await _supabaseService.GetCartItemsByUserId(userId);
    return Ok(cartItemsWithProducts);
}


        // POST: api/cart/add
  [HttpPost("add")]
public async Task<IActionResult> AddToCart([FromBody] Cart cart)
{
    // Add the cart item using your service
Console.WriteLine($"Hello {(cart)}");
    
    var addedCartItem = await _supabaseService.AddToCart(cart);
            Console.WriteLine($"Added: {Newtonsoft.Json.JsonConvert.SerializeObject(addedCartItem)}");


    return Ok(addedCartItem);
}


        // PUT: api/cart/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCartItem(int id, int quantity)
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