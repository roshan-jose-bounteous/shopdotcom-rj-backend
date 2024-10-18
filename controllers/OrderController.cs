using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shopdotcobackend.Models;
using shopdotcobackend.Services;

namespace shopdotcobackend.controllers
{
    [Route("api/order/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly SupabaseService _supabaseService;

        public OrderController(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

          [HttpPost("AddToOrders")]
        public async Task<IActionResult> AddToOrders([FromBody] Order order)
        {

                 Console.WriteLine($"Response total value: {Newtonsoft.Json.JsonConvert.SerializeObject(order)}");
                 Console.WriteLine($"User ID: {order.user_id}");

            if (order == null)
            {
                return BadRequest("Order details are missing.");
            }

            var result = await _supabaseService.AddToOrders(order);


            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the order.");
            }
        }

        
    }
}