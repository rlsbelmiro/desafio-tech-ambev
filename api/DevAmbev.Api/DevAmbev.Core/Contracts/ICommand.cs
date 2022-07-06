using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Commands.Contracts
{
    public interface ICommand<T, E>
    {
        public Task<E> Handle(T request, string emailUsuario);
    }
}
