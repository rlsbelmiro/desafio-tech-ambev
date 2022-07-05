using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using DevAmbev.Domain.Entities;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Commands.Users
{
    public class CreateUserCommand : ICommand<UserRequest, UserResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;

        public CreateUserCommand(IMapper mapper, IUserRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Handle(UserRequest request)
        {
            var response = new UserResponse();
            try
            {
                var entity = _mapper.Map<User>(request);
                entity.CreatedAt = DateTime.Now;
                entity.CreatedBy = "renato.belmiro@rbcoding.com.br";
                entity.Active = true;
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
                        response = _mapper.Map<UserResponse>(entity);
                        response.Success = true;
                        response.Message = "Operação realizada com sucesso!";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Erro ao inserir usuário";
                    }
                }

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception("Erro ao criar usuário: ");
            }
        }
    }
}
