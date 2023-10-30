using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ASPNETCoreRestaurantApplication.Models
{
    [Table("customers")]
    public class Customer
    {
        [Key]
        public int customer_id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string phone_number { get; set; }

        // Properti navigasi
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
    }
}
