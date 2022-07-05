using AutoMapper;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Queries.Users
{
    public class FindByIdUserQuery : IQuery<int, UserResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public FindByIdUserQuery(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Handle(int request)
        {
            var response = new UserResponse();
            try
            {
                if (request == 0)
                    throw new Exception("Informe o Id do usuário");

                var user = await _repository.GetById(request);
                if (user.Id == 0)
                    throw new Exception("Usuário não encontrado");

                response = _mapper.Map<UserResponse>(user);
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar usuário: " + ex.Message;
            }
            return response;
        }
    }
}
