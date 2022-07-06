using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Orders
{
    public class OrderItemRequest
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
