using AutoMapper;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Queries.Products
{
    public class ListProductQuery : IQuery<ProductListRequest, ProductListResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ListProductQuery(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductListResponse> Handle(ProductListRequest request)
        {
            var response = new ProductListResponse();
            try
            {
                var users = await _repository.List();
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                response.Products = _mapper.Map<List<ProductResponse>>(users);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar produtos: " + ex.Message;
            }
            return response;
        }
    }
}
