using DevAmbev.Core.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Orders
{
    public class OrderResponse : BaseResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public IEnumerable<OrderItemResponse> Items { get; set; }

    }
}
