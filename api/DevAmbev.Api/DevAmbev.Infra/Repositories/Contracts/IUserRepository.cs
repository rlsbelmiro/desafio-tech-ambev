using DevAmbev.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Infra.Repositories.Contracts
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> Login(string email, string password);
    }
}
