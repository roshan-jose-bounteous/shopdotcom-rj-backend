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

        

//          [HttpGet("sort")]
//     public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySort(string sort)
//     {
//         var products = await _supabaseService.GetProductsBySort(sort);

//         if (products == null || !products.Any())
//         {
//             return NotFound("No products found.");
//         }

//         return Ok(products);
//     }



//         [HttpGet("filter")]
// public async Task<ActionResult<IEnumerable<Product>>> GetProductsByFilter(string? tags, string? sizes, string? sort)
// {
//     // Get all products if no filters or sort specified
//     if (string.IsNullOrEmpty(tags) && string.IsNullOrEmpty(sizes))
//     {
//         return await GetProductsBySort(sort); 
//     }

//     var products = await _supabaseService.GetProductsByFilter(tags, sizes, sort);
//     if (products == null || !products.Any())
//     {
//         return NotFound("No products found for the specified filters.");
//     }

//     return Ok(products);
// }


// [HttpPost("related-by-tags")]
// public async Task<ActionResult<IEnumerable<Product>>> GetRelatedProductsByTags([FromBody] List<string> tags, [FromQuery] int excludedProductId)
// {
//     if (tags == null || !tags.Any())
//     {
//         return BadRequest("Tags list cannot be null or empty.");
//     }

//     var products = await _supabaseService.GetRelatedProductsByTags(tags, excludedProductId);

//     if (products == null || !products.Any())
//     {
//         return NotFound("No related products found.");
//     }

//     return Ok(products);
// }


[HttpGet("products")]
public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
    string? sort = null, 
    string? tags = null, 
    string? sizes = null, 
    [FromQuery] List<string>? relatedTags = null, 
    int? excludedProductId = null)
{
    // Case 1: Fetch related products based on tags
    if (relatedTags != null && relatedTags.Any() && excludedProductId.HasValue)
    {
        var relatedProducts = await _supabaseService.GetRelatedProductsByTags(relatedTags, excludedProductId.Value);
        if (relatedProducts == null || !relatedProducts.Any())
        {
            return NotFound("No related products found.");
        }
        return Ok(relatedProducts);
    }

    // Case 2: Get products filtered by tags, sizes, and optionally sorted
    if (!string.IsNullOrEmpty(tags) || !string.IsNullOrEmpty(sizes))
    {
        var filteredProducts = await _supabaseService.GetProductsByFilter(tags, sizes, sort);
        if (filteredProducts == null || !filteredProducts.Any())
        {
            return NotFound("No products found for the specified filters.");
        }
        return Ok(filteredProducts);
    }

    // Case 3: Get products by sorting if no filters are provided
    var sortedProducts = await _supabaseService.GetProductsBySort(sort);
    if (sortedProducts == null || !sortedProducts.Any())
    {
        return NotFound("No products found.");
    }

    return Ok(sortedProducts);
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
            return Ok("Deleted Succesfully"); 
        }
    }
}