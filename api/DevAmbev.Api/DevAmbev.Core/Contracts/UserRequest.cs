using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Pasword { get; set; }
    }
}
