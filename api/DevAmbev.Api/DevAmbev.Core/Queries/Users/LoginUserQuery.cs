using AutoMapper;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Queries.Users
{
    public class LoginUserQuery : IQuery<LoginUserRequest, LoginUserResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LoginUserQuery(IUserRepository repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = config;
        }

        public async Task<LoginUserResponse> Handle(LoginUserRequest request)
        {
            var response = new LoginUserResponse();
            try
            {
                if(string.IsNullOrEmpty(request.Email))
                    throw new Exception("Informe o Email do usuário");
                if (string.IsNullOrEmpty(request.Password))
                    throw new Exception("Informe a senha do usuário");

                var user = await _repository.Login(request.Email, request.Password);
                if (user.Id == 0)
                    throw new Exception("Usuário não encontrado");

                response = _mapper.Map<LoginUserResponse>(user);
                response.Token = "token_login";
                response.Success = true;
                response.Message = "Operação realizada com sucesso";
                
            }
            catch(Exception ex)
            {
                throw new Exception(_configuration.GetConnectionString("SqlConnection"));
                response.Success = false;
                response.Message = "Erro ao consultar usuário: " + ex.Message;
            }
            return response;
        }
    }
}
