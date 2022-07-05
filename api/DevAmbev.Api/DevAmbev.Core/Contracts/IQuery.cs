using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Contracts
{
    public interface IQuery<T, E>
    {
        Task<E> Handle(T request);
    }
}
