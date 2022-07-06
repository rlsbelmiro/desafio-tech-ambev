using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Commands.Customers
{
    public class DeleteCustomerCommand : ICommand<int, BaseResponse>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public DeleteCustomerCommand(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> Handle(int request, string emailUsuario)
        {
            var response = new BaseResponse();
            try
            {
                if (request == 0)
                    throw new Exception("Informe o Id do cliente");

                var entity = await _repository.GetById(request);
                if (entity.Id == 0)
                    throw new Exception("Cliente não encontrado");

                var result = await _repository.Delete(request);

                
                response.Success = true;
                response.Message = "Cliente excluído com sucesso!";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao excluir cliente: " + ex.Message;
            }
            return response;
        }
    }
}
