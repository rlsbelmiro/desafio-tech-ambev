using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Domain.Entities;
using DevAmbev.Infra.Repositories.Contracts;

namespace DevAmbev.Core.Commands.Customers
{
    public class CreateCustomerCommand : ICommand<CustomerRequest, CustomerResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommand(IMapper mapper, ICustomerRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> Handle(CustomerRequest request)
        {
            var response = new CustomerResponse();
            try
            {
                var entity = _mapper.Map<Customer>(request);
                entity.CreatedAt = DateTime.Now;
                entity.CreatedBy = "renato.belmiro@rbcoding.com.br";
                var validate = entity.Validate();
                if (!validate)
                {
                    response.Success = false;
                    response.Message = String.Join(", ", entity.ListOfError);
                }
                else
                {
                    var customerExist = await _repository.GetByDocument(request.Document);
                    if (customerExist.Id == 0)
                    {
                        var result = await _repository.Add(entity);

                        if (result)
                        {
                            response = _mapper.Map<CustomerResponse>(entity);
                            response.Success = true;
                            response.Message = "Operação realizada com sucesso!";
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Erro ao inserir cliente";
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Cliente já cadastrado com este documento";
                    }
                }

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception("Erro ao criar cliente: " + ex.Message);
            }
        }
    }
}
