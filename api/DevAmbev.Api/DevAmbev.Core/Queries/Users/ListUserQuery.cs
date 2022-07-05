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
    public class ListUserQuery : IQuery<UserListRequest, UserListResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public ListUserQuery(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserListResponse> Handle(UserListRequest request)
        {
            var response = new UserListResponse();
            try
            {
                var users = await _repository.List();
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                response.Users = _mapper.Map<List<UserResponse>>(users);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao consultar usuários: " + ex.Message;
            }
            return response;
        }
    }
}
