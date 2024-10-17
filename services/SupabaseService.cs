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

        // Get all products
        public async Task<List<Product>> GetAllProducts()
        {
            var response = await _supabaseClient.From<Product>().Get();
            
            return response.Models;
        }

        // // Get product by ID
        public async Task<Product?> GetProductById(int id)
        {
            var response = await _supabaseClient.From<Product>().Where(x=>x.id==id).Single();
            return response;
        }

        // Get Product by Tags
        //  public async Task<List<Product>> GetProductsByTags(string tags)
        // {
        //     var response = await _supabaseClient.From<Product>()
        // .Where(x => x.tags.Contains(tags))
        // .Get();

        //     return response.Models;
        // }

         public async Task<List<Product>> GetProductsByFilter(string? tags, string? sizes)
         
        {

            if(!string.IsNullOrEmpty(tags) && !string.IsNullOrEmpty(sizes))
            {

                var response = await _supabaseClient.From<Product>()
                    .Where(x => x.tags.Contains(tags) &&
                                 x.sizes.Contains(sizes))
                    .Get();
 

                return response.Models;
            }
            else if (!string.IsNullOrEmpty(tags))
            {
                var response = await _supabaseClient.From<Product>()
                    .Where(x => x.tags.Contains(tags))
                    .Get();

                return response.Models;
            }
            else if (!string.IsNullOrEmpty(sizes))
            {
                var response = await _supabaseClient.From<Product>()
                    .Where(x => x.sizes.Contains(sizes))
                    .Get();

                return response.Models;
            }

            return null;
        }



        // // Add a new product
        public async Task<Product?> AddProduct(Product product)
        {
            var response = await _supabaseClient.From<Product>().Insert(product);
            return response.Model;
        }

        

        public async Task<bool> DeleteProduct(int id)
        {
            try{
            await _supabaseClient.From<Product>().Where(x => x.id == id).Delete();
            return true;
            }
            catch(Exception){
                return false;
                throw;
            }
        }



        public async Task<List<Cart>> GetCartItemsByUserId(string user_id)
        {
            var response = await _supabaseClient.From<Cart>()
                .Where(x => x.user_id == user_id)
                .Get();

            // Populate Product details for each Cart item
            foreach (var cartItem in response.Models)
            {
                var productResponse = await _supabaseClient.From<Product>()
                    .Where(p => p.id == cartItem.product_id)
                    .Single();

                if (productResponse != null)
                {
                    cartItem.Product = productResponse;
                }
            }

            return response.Models;
        }

        // Add an item to the cart
        public async Task<Cart?> AddToCart(Cart cart)
        {
            var response = await _supabaseClient.From<Cart>().Insert(cart);
            return response.Model;
        }

        // Update the quantity of a cart item
        public async Task<Cart?> UpdateCartItem(int id, int quantity)
        {
            var cartItem = new Cart { id = id, quantity = quantity };
            var response = await _supabaseClient.From<Cart>().Where(x => x.id == id).Update(cartItem);
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


    }
}