using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Infra.Repositories.Contracts
{
    public interface IBaseRepository<T>
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> List();
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
    }
}
