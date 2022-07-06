using AutoMapper;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Queries.Customers
{
    public class FindByIdCustomerQuery : IQuery<int, CustomerResponse>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public FindByIdCustomerQuery(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> Handle(int request)
        {
            var response = new CustomerResponse();
            try
            {
                if (request == 0)
                    throw new Exception("Informe o Id do cliente");

                var customer = await _repository.GetById(request);
                if (customer.Id == 0)
                    throw new Exception("Cliente não encontrado");

                response = _mapper.Map<CustomerResponse>(customer);
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar cliente: " + ex.Message;
            }
            return response;
        }
    }
}
