using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Commands.Customers
{
    public class UpdateCustomerCommand : ICommand<CustomerUpdateRequest, CustomerResponse>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommand(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> Handle(CustomerUpdateRequest request, string emailUsuario)
        {
            var response = new CustomerResponse();
            try
            {
                if (request.Id == 0)
                    throw new Exception("Informe o Id do cliente a ser alterado");

                var entity = await _repository.GetById(request.Id);
                if (entity.Id == 0)
                    throw new Exception("Cliente não encontrado");

                entity.Name = request.Name;
                entity.Email = request.Email;
                entity.Active = request.Active;
                entity.Document = request.Document;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = emailUsuario;

                var validate = entity.Validate();
                if (!validate)
                    throw new Exception(String.Join(", ", entity.ListOfError));

                var customerExist = await _repository.GetByDocument(request.Document);
                if (customerExist.Id > 0 && customerExist.Id != request.Id)
                {
                    throw new Exception("Cliente já cadastrado com o mesmo documento");
                }

                var result = await _repository.Update(entity);

                response = _mapper.Map<CustomerResponse>(entity);
                response.Success = true;
                response.Message = "Cliente alterado com sucesso!";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao editar cliente: " + ex.Message;
            }
            return response;
        }
    }
}
