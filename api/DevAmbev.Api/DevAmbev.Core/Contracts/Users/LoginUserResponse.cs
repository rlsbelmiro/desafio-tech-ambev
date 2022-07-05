using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Users
{
    public class LoginUserResponse : UserResponse
    {
        public string Token { get; set; }
    }
}
