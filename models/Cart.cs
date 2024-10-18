// Models/Cart.cs
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace shopdotcobackend.Models
{
    [Table("cart")] // Match the table name in the Supabase database
    public class Cart : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("user_id")]
        public string user_id { get; set; }

        [Column("product_id")]
        public int product_id { get; set; }

        [Column("quantity")]
        public int quantity { get; set; }

        [Column("size")]
        public string size { get; set; }

        [Column("is_placed")]
        public bool is_placed { get; set; } = false;

    }
}
