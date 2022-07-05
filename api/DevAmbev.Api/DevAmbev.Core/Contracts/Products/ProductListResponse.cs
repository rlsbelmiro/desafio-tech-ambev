using DevAmbev.Core.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Products
{
    public class ProductListResponse : BaseResponse
    {
        public IEnumerable<ProductResponse> Products { get; set; }
    }
}
