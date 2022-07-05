﻿using DevAmbev.Core.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts.Users
{
    public class UserListResponse : BaseResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}
