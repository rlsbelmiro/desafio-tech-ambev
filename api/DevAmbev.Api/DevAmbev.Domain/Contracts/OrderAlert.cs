using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Domain.Contracts
{
    public class OrderAlert
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int OrderId { get; set; }
        public decimal OrderAmount { get; set; }
        public IEnumerable<OrderAlertItems> OrderItems { get; set; }

    }
}
