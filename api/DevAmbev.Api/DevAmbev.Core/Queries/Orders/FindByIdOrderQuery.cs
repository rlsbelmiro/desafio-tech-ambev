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

namespace DevAmbev.Core.Queries.Customers
{
    public class FindByIdOrderQuery : IQuery<int, OrderResponse>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public FindByIdOrderQuery(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(int request)
        {
            var response = new OrderResponse();
            try
            {
                if (request == 0)
                    throw new Exception("Informe o Id do pedido");

                var order = await _repository.GetById(request);
                if (order.Id == 0)
                    throw new Exception("Pedido não encontrado");

                response = _mapper.Map<OrderResponse>(order);
                response.Items = _mapper.Map<List<OrderItemResponse>>(order.OrderItems);
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar pedido: " + ex.Message;
            }
            return response;
        }
    }
}
