// Models/Cart.cs
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace shopdotcobackend.Models
{
    
    public class CartItemWithProduct : BaseModel
{
    public Cart CartItem { get; set; }
    public Product Product { get; set; }
}
}
