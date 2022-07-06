using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Customers
{
    public class CustomerRequest
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}
