using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Domain.Entities;
using DevAmbev.Infra.Repositories.Contracts;

namespace DevAmbev.Core.Commands.Products
{
    public class CreateProductCommand : ICommand<ProductRequest, ProductResponse>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;

        public CreateProductCommand(IMapper mapper, IProductRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> Handle(ProductRequest request)
        {
            var response = new ProductResponse();
            try
            {
                var entity = _mapper.Map<Product>(request);
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
                    var result = await _repository.Add(entity);

                    if (result)
                    {
                        response = _mapper.Map<ProductResponse>(entity);
                        response.Success = true;
                        response.Message = "Operação realizada com sucesso!";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Erro ao inserir produto";
                    }
                }

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception("Erro ao criar product: " + ex.Message);
            }
        }
    }
}
