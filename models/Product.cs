using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace shopdotcobackend.Models
{
    
     [Table("products")] // Ensure this matches the table name in your Supabase database
    public class Product : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("price")]
        public decimal price { get; set; }

        [Column("thumbnail")]
        public string thumbnail { get; set; }

        [Column("details")]
        public string details { get; set; }

        
        [Column("images")]
        public List<Image> images { get; set; } = [];


        [Column("tags")]
        public List<string> tags { get; set; } = [];

        [Column("sizes")]
        public List<string> sizes { get; set; } = [];


        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public class Image
    {
        public string alt { get; set; } = string.Empty;
        public string imageurl { get; set; } = string.Empty;
    }
}


