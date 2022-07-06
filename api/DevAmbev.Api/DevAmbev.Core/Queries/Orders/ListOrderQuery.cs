using AutoMapper;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Orders;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Queries.Orders
{
    public class ListOrderQuery : IQuery<OrderListRequest, OrderListResponse>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public ListOrderQuery(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderListResponse> Handle(OrderListRequest request)
        {
            var response = new OrderListResponse();
            try
            {
                var orders = await _repository.List();
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                response.Orders = _mapper.Map<List<OrderResponse>>(orders);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar pedidos: " + ex.Message;
            }
            return response;
        }
    }
}
