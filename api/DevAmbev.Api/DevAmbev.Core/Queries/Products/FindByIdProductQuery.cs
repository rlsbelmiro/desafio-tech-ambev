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
    public class FindByIdProductQuery : IQuery<int, ProductResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public FindByIdProductQuery(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> Handle(int request)
        {
            var response = new ProductResponse();
            try
            {
                if (request == 0)
                    throw new Exception("Informe o Id do produto");

                var user = await _repository.GetById(request);
                if (user.Id == 0)
                    throw new Exception("Produto não encontrado");

                response = _mapper.Map<ProductResponse>(user);
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar produto: " + ex.Message;
            }
            return response;
        }
    }
}
