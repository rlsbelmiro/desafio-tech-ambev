﻿using AutoMapper;
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
    public class DeleteProductCommand : ICommand<int, ProductResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public DeleteProductCommand(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> Handle(int request, string emailUsuario)
        {
            var response = new ProductResponse();
            try
            {
                if (request == 0)
                    throw new Exception("Informe o Id do produto");

                var entity = await _repository.GetById(request);
                if (entity.Id == 0)
                    throw new Exception("Produto não encontrado");

                var result = await _repository.Delete(request);

                
                response.Success = true;
                response.Message = "Produto excluído com sucesso!";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao excluir produto: " + ex.Message;
            }
            return response;
        }
    }
}
