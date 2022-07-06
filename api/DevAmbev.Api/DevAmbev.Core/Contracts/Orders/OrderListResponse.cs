using DevAmbev.Core.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Orders
{
    public class OrderListResponse : BaseResponse
    {
        public IEnumerable<OrderResponse> Orders { get; set; }
    }
}
