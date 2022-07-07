using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Orders
{
    public class OrderItemResponse
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductName { get; set; }
    }
}
