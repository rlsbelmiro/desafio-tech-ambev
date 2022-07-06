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
    public class ListCustomerQuery : IQuery<CustomerListRequest, CustomerListResponse>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public ListCustomerQuery(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerListResponse> Handle(CustomerListRequest request)
        {
            var response = new CustomerListResponse();
            try
            {
                var users = await _repository.List();
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                response.Customers = _mapper.Map<List<CustomerResponse>>(users);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar clientes: " + ex.Message;
            }
            return response;
        }
    }
}
