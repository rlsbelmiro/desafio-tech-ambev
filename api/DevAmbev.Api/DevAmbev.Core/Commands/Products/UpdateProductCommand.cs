using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Commands.Products
{
    public class UpdateProductCommand : ICommand<ProductUpdateRequest, ProductResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public UpdateProductCommand(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> Handle(ProductUpdateRequest request, string emailUsuario)
        {
            var response = new ProductResponse();
            try
            {
                if (request.Id == 0)
                    throw new Exception("Informe o Id do producto a ser alterado");

                var entity = await _repository.GetById(request.Id);
                if (entity.Id == 0)
                    throw new Exception("Usuário não encontrado");

                entity.Name = request.Name;
                entity.Active = request.Active;
                entity.Description = request.Description;
                entity.Price = request.Price;
                entity.Quantity = request.Quantity;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = emailUsuario;

                var validate = entity.Validate();
                if (!validate)
                    throw new Exception(String.Join(", ", entity.ListOfError));

                var result = await _repository.Update(entity);

                response = _mapper.Map<ProductResponse>(entity);
                response.Success = true;
                response.Message = "Produto alterado com sucesso!";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao editar produto: " + ex.Message;
            }
            return response;
        }
    }
}
