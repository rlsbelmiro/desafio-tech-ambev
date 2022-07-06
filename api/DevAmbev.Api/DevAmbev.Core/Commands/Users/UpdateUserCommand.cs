using AutoMapper;
using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts.Users;
using DevAmbev.Infra.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevAmbev.Core.Commands.Users
{
    public class UpdateUserCommand : ICommand<UserUpdateRequest, UserResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUserCommand(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Handle(UserUpdateRequest request, string emailUsuario)
        {
            var response = new UserResponse();
            try
            {
                if (request.Id == 0)
                    throw new Exception("Informe o Id do usuário a ser alterado");

                var entity = await _repository.GetById(request.Id);
                if (entity.Id == 0)
                    throw new Exception("Usuário não encontrado");

                if(entity.Email != request.Email)
                {
                    var userExist = await _repository.GetUserByEmail(request.Email);
                    if (userExist.Id > 0 && userExist.Id != request.Id)
                        throw new Exception("Usuário já cadastrado com este email");
                }

                entity.Email = request.Email;
                entity.Name = request.Name;
                entity.Active = request.Active;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = emailUsuario;

                var validate = entity.Validate();
                if (!validate)
                    throw new Exception(String.Join(", ", entity.ListOfError));

                var result = await _repository.Update(entity);

                response = _mapper.Map<UserResponse>(entity);
                response.Success = true;
                response.Message = "Usuário alterado com sucesso!";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = "Erro ao editar usuário: " + ex.Message;
            }
            return response;
        }
    }
}
