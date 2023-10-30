using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreRestaurantApplication.NewFolder
{
    public class TransactionResponse
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public int FoodId { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
