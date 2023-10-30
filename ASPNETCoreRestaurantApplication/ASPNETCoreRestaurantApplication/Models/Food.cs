using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreRestaurantApplication.Models
{
    [Table("foods")]
    public class Food
    {
        [Key]
        public int food_id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public int price { get; set; }

        public string category { get; set; }

        // Properti navigasi

        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }

    }
}
