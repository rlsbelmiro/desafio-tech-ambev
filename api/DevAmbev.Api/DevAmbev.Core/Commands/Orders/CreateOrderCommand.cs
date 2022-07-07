using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts.Orders;
using DevAmbev.Domain.Entities;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Commands.Orders
{
    public class CreateOrderCommand : ICommand<OrderRequest, OrderResponse>
    {
        private readonly IOrderRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CreateOrderCommand(IOrderRepository repository, IMapper mapper, IProductRepository productRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<OrderResponse> Handle(OrderRequest request, string emailUsuario)
        {
            var response = new OrderResponse();
            try
            {
                var entity = _mapper.Map<Order>(request);
                entity.OrderItems = _mapper.Map<List<OrderItem>>(request.Items);
                entity.CreatedBy = emailUsuario;
                entity.CreatedAt = DateTime.Now;
                entity.Active = true;

                await this.SumTotalPrice(entity);

                var validate = entity.Validate();
                if (!validate)
                    throw new Exception(string.Join(", ", entity.ListOfError));

                var result = await _repository.Add(entity);
                if (result)
                {
                    response.Id = entity.Id;
                    response.Amount = entity.Amount;
                    response.Success = true;
                    response.Message = "Pedido criado com sucesso";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao criar pedido: " + ex.Message;
            }
            return response;
        }

        private async Task SumTotalPrice(Order order)
        {
            decimal total = 0;
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetById(item.ProductId);
                item.UnityPrice = product.Price;
                total += item.TotalPrice;
            }
            order.Amount = total;
        }
    }
}
