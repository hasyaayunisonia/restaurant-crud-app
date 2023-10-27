using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreRestaurantApplication.Models
{
    [Table("tb_transaction")]
    public class Transaction
    {
        [Key]
        public int transaction_id { get; set; }

        [ForeignKey("Customer")]
        public int customer_id { get; set; }

        [ForeignKey("Food")]
        public int food_id { get; set; }

        public decimal amount { get; set; }

        public DateTime order_date { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }

        public Food Food { get; set; }
    }
}
