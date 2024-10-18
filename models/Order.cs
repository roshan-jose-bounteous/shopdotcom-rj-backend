
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace shopdotcobackend.Models
{
    [Table("orders")] 
    public class Order : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("cart_id")]
        public List<int> cart_id { get; set; } = [];

        [Column("user_id")]
        public string user_id { get; set; }


        [Column("name")]
        public string name { get; set; }

        [Column("address")]
        public string address { get; set; }

        [Column("pincode")]
        public string pincode { get; set; }

        [Column("city")]
        public string city { get; set; }

        [Column("state")]
        public string state { get; set; }

        [Column("phone_no")]
        public string phone_no { get; set; }

                [Column("payment_mode")]
        public string payment_mode { get; set; }




    }
}
