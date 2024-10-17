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
    public class ProductController : ControllerBase
    {
         private readonly SupabaseService _supabaseService;

        public ProductController(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _supabaseService.GetAllProducts();

            var serializedProducts = JsonConvert.SerializeObject(products);
            return Ok(serializedProducts);

        }

        // GET: api/product/tags

        // [HttpGet("filter")]
        // public async Task<ActionResult<IEnumerable<Product>>> GetProductsByFilter(string? tags, string? sizes)
        // {

        //     if (string.IsNullOrEmpty(tags) && string.IsNullOrEmpty(sizes))
        //     {
        //         return await GetAllProducts();
        //     }

        //     var products = await _supabaseService.GetProductsByFilter(tags, sizes);
        //     if (products == null || !products.Any())
        //     {
        //         return NotFound("No products found for the specified filters.");
        //     }

        //     return Ok(products);
        // }

         [HttpGet("sort")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySort(string sort)
    {
        var products = await _supabaseService.GetProductsBySort(sort);

        if (products == null || !products.Any())
        {
            return NotFound("No products found.");
        }

        return Ok(products);
    }

    

        [HttpGet("filter")]
public async Task<ActionResult<IEnumerable<Product>>> GetProductsByFilter(string? tags, string? sizes, string? sort)
{
    // Get all products if no filters or sort specified
    if (string.IsNullOrEmpty(tags) && string.IsNullOrEmpty(sizes))
    {
        return await GetProductsBySort(sort); // Call the new GetProductsBySort method
    }

    var products = await _supabaseService.GetProductsByFilter(tags, sizes, sort);
    if (products == null || !products.Any())
    {
        return NotFound("No products found for the specified filters.");
    }

    return Ok(products);
}



        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _supabaseService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            var serializedProduct = JsonConvert.SerializeObject(product);

            return Ok(serializedProduct);
        }


// GET: api/product/tags
        // [HttpGet("tags")]
        // public async Task<ActionResult<IEnumerable<Product>>> GetProductsByTags(string tags)
        // {
        //     if (tags == null || !tags.Any())
        //     {
        //         return BadRequest("Tags are required.");
        //     }

        //     var products = await _supabaseService.GetProductsByTags(tags);
        //     if (products == null || !products.Any())
        //     {
        //         return NotFound("No products found for the specified tags.");
        //     }

        //     return Ok(products);
        // }
        // POST: api/product
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            var addedProduct = await _supabaseService.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.id }, addedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var deleted = await _supabaseService.DeleteProduct(id);
            if (!deleted)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok("Deleted Succesfully"); // 204 No Content response
        }
    }
}