using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopdotcobackend.Models;
using Supabase;



namespace shopdotcobackend.Services
{
    public class SupabaseService
    {
        private readonly Client _supabaseClient;

        public SupabaseService(Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }



        // public async Task<session?> LoginAsync(string email, string password)
        // {
        //     var response = await _supabaseClient.Auth.SignInWithPassword(email, password);
        //     return response;
        // }

        // public async Task LogoutAsync()
        // {
        //     await _supabaseClient.Auth.SignOutAsync();
        // }


        // Get all products
        public async Task<List<Product>> GetAllProducts()
        {
            var response = await _supabaseClient.From<Product>().Get();

            return response.Models;
        }

        // // Get product by ID
        public async Task<Product?> GetProductById(int id)
        {
            var response = await _supabaseClient.From<Product>().Where(x => x.id == id).Single();
            return response;
        }


        public async Task<List<Product>> GetProductsBySort(string sort)
        {
            var response = await _supabaseClient.From<Product>().Get();
            var products = response.Models;

            // Sorting logic
            if (products == null || !products.Any())
            {
                return new List<Product>(); // Return an empty list if no products found
            }

            switch (sort)
            {
                case "price-high-to-low":
                    return products.OrderByDescending(p => p.price).ToList();

                case "price-low-to-high":
                    return products.OrderBy(p => p.price).ToList();

                case "alphabetical":
                    return products.OrderBy(p => p.name).ToList();

                case "most-popular":
                    return products;
                default:
                    return products;
            }
        }


        public async Task<List<Product>> GetRelatedProductsByTags(List<string> tags, int excludedProductId)
        {
            var response = await _supabaseClient.From<Product>().Get();
            var products = response.Models;

            if (products == null || !products.Any())
            {
                return new List<Product>(); // Return an empty list if no products found
            }

            // Filter products by checking if their tags match the given tags and exclude the specified ID
            return products
                .Where(p => p.id != excludedProductId && p.tags != null && p.tags.Any(tag => tags.Contains(tag, StringComparer.OrdinalIgnoreCase)))
                .ToList();
        }


        // Get Product by Tags
        //  public async Task<List<Product>> GetProductsByTags(string tags)
        // {
        //     var response = await _supabaseClient.From<Product>()
        // .Where(x => x.tags.Contains(tags))
        // .Get();

        //     return response.Models;
        // }

        //  public async Task<List<Product>> GetProductsByFilter(string? tags, string? sizes)

        // {

        //     if(!string.IsNullOrEmpty(tags) && !string.IsNullOrEmpty(sizes))
        //     {

        //         var response = await _supabaseClient.From<Product>()
        //             .Where(x => x.tags.Contains(tags) &&
        //                          x.sizes.Contains(sizes))
        //             .Get();


        //         return response.Models;
        //     }
        //     else if (!string.IsNullOrEmpty(tags))
        //     {
        //         var response = await _supabaseClient.From<Product>()
        //             .Where(x => x.tags.Contains(tags))
        //             .Get();

        //         return response.Models;
        //     }
        //     else if (!string.IsNullOrEmpty(sizes))
        //     {
        //         var response = await _supabaseClient.From<Product>()
        //             .Where(x => x.sizes.Contains(sizes))
        //             .Get();

        //         return response.Models;
        //     }

        //     return null;
        // }

        public async Task<List<Product>> GetProductsByFilter(string? tags, string? sizes, string? sort)
        {
            List<Product> products;

            // Get products based on filters
            if (!string.IsNullOrEmpty(tags) && !string.IsNullOrEmpty(sizes))
            {
                var response = await _supabaseClient.From<Product>()
                    .Where(x => x.tags.Contains(tags) && x.sizes.Contains(sizes))
                    .Get();

                products = response.Models;
            }
            else if (!string.IsNullOrEmpty(tags))
            {
                var response = await _supabaseClient.From<Product>()
                    .Where(x => x.tags.Contains(tags))
                    .Get();

                products = response.Models;
            }
            else if (!string.IsNullOrEmpty(sizes))
            {
                var response = await _supabaseClient.From<Product>()
                    .Where(x => x.sizes.Contains(sizes))
                    .Get();

                products = response.Models;
            }
            else
            {
                // If no filters, get all products
                var allProductsResponse = await _supabaseClient.From<Product>().Get();
                products = allProductsResponse.Models;
            }

            // Sorting logic
            if (products != null)
            {
                switch (sort)
                {
                    case "price-high-to-low":
                        products = products.OrderByDescending(p => p.price).ToList();
                        break;

                    case "price-low-to-high":
                        products = products.OrderBy(p => p.price).ToList();
                        break;

                    case "alphabetical":
                        products = products.OrderBy(p => p.name).ToList();
                        break;

                    case "most-popular":
                        return products;
                }
            }

            return products;
        }




        // // Add a new product
        public async Task<Product?> AddProduct(Product product)
        {
            var response = await _supabaseClient.From<Product>().Insert(product);
            return response.Model;
        }



        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                await _supabaseClient.From<Product>().Where(x => x.id == id).Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }



        // public async Task<List<Cart>> GetCartItemsByUserId(string user_id)
        // {
        //     var response = await _supabaseClient.From<Cart>()
        //         .Where(x => x.user_id == user_id)
        //         .Get();



        //     return response.Models;
        // }


        // public async Task<List<CartItemWithProduct>> GetCartItemsByUserId(string user_id)
        // {
        //     var cartResponse = await _supabaseClient.From<Cart>()
        //         .Where(x => x.user_id == user_id)
        //         .Get();

        //     var cartItems = cartResponse.Models;
        //     var cartItemsWithProducts = new List<CartItemWithProduct>();

        //     foreach (var cartItem in cartItems)
        //     {
        //         var product = await _supabaseClient.From<Product>()
        //             .Where(x => x.id == cartItem.product_id)
        //             .Single();

        //         if (product != null)
        //         {
        //             cartItemsWithProducts.Add(new CartItemWithProduct
        //             {
        //                 CartItem = cartItem,
        //                 Product = product
        //             });
        //         }
        //     }

        //     return cartItemsWithProducts;
        // }

        public async Task<List<CartItemWithProduct>> GetCartItemsByUserId(string user_id)
{
    var cartResponse = await _supabaseClient.From<Cart>()
        .Where(x => x.user_id == user_id && x.is_placed == false)
        .Order("created_at",  Supabase.Postgrest.Constants.Ordering.Ascending) // Use Order with OrderDirection
        .Get();

    var cartItems = cartResponse.Models;
    var cartItemsWithProducts = new List<CartItemWithProduct>();

    foreach (var cartItem in cartItems)
    {
        var product = await _supabaseClient.From<Product>()
            .Where(x => x.id == cartItem.product_id)
            .Single();

        if (product != null)
        {
            cartItemsWithProducts.Add(new CartItemWithProduct
            {
                CartItem = cartItem,
                Product = product
            });
        }
    }

    return cartItemsWithProducts;
}


        // Add an item to the cart
        // public async Task<Cart?> AddToCart(Cart cart)
        // {
        //     var response = await _supabaseClient.From<Cart>().Insert(cart);
        //     return response.Model;
        // }

        public async Task<Cart?> AddToCart(Cart cart)
        {


            var response = await _supabaseClient.From<Cart>().Insert(cart);
            Console.WriteLine($"Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
            return response.Model;
        }


        // Update the quantity of a cart item
        public async Task<Cart?> UpdateCartItem(int id, int quantity)
        {
            var response = await _supabaseClient.From<Cart>().Where(x => x.id == id).Set(x => x.quantity, quantity).Update();
            return response.Model;
        }

        // Delete a cart item
        public async Task<bool> DeleteCartItem(int id)
        {
            try
            {
                await _supabaseClient.From<Cart>().Where(x => x.id == id).Delete();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


// public async Task<Order?> AddToOrders(Order order)
// {
//     try
//     {
//         Console.WriteLine($"inside: {Newtonsoft.Json.JsonConvert.SerializeObject(order)}");
        
//         var response = await _supabaseClient.From<Order>().Insert(order);
//         Console.WriteLine($"order response: {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
//         return response.Model;
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error occurred: {ex.Message}");
//         // Optionally log the stack trace or more details
//         throw; // Rethrow or handle accordingly
//     }
// }

public async Task<Order?> AddToOrders(Order order)
{
    try
    {
        Console.WriteLine($"Inside: {Newtonsoft.Json.JsonConvert.SerializeObject(order)}");

        // Insert the order
        var response = await _supabaseClient.From<Order>().Insert(order);
        Console.WriteLine($"Order response: {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");

        // Update the is_placed status for each cart item
        foreach (var cartId in order.cart_id)
        {
            var updateResponse = await _supabaseClient.From<Cart>()
                .Where(c => c.id == cartId)
                .Set(c => c.is_placed, true) // Set is_placed to true
                .Update();

            if (updateResponse == null)
            {
                Console.WriteLine($"Failed to update cart item with ID {cartId}");
            }
        }

        return response.Model;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred: {ex.Message}");
        // Optionally log the stack trace or more details
        throw; // Rethrow or handle accordingly
    }
}




    }
}