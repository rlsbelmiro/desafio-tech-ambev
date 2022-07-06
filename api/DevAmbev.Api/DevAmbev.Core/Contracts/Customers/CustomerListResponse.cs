using DevAmbev.Core.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Customers
{
    public class CustomerListResponse : BaseResponse
    {
        public IEnumerable<CustomerResponse> Customers { get; set; }
    }
}
