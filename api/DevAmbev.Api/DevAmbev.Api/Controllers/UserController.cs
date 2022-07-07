using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevAmbev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        public UserController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse>> CreateUser([FromServices] ICommand<UserRequest, UserResponse> command, [FromBody] UserRequest userRequest)
        {
            try
            {
                return Ok(await command.Handle(userRequest, User.Identity.Name));
            }
            catch(Exception ex)
            {
                var messageRequest = JsonConvert.SerializeObject(userRequest);
                _logger.LogError($"Erro ao executar CreateUser: {ex.Message}", messageRequest);
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> EditUser([FromServices] ICommand<UserUpdateRequest, UserResponse> command, [FromBody] UserUpdateRequest request, [FromRoute] int id)
        {
            try
            {
                request.Id = id;
                return Ok(await command.Handle(request, User.Identity.Name));
            }
            catch(Exception ex)
            {
                var messageRequest = JsonConvert.SerializeObject(request);
                _logger.LogError($"Erro ao executar EditUser: {ex.Message}", messageRequest);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> DeleteUser([FromServices] ICommand<int, BaseResponse> command, [FromRoute] int id)
        {
            try
            {
                return Ok(await command.Handle(id, User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar DeleteUser: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserListResponse>> List([FromServices] IQuery<UserListRequest, UserListResponse> query)
        {
            try
            {
                return Ok(await query.Handle(new UserListRequest()));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao executar ListUser: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<UserListResponse>> GetById([FromServices] IQuery<int, UserResponse> query, [FromRoute] int id)
        {
            try
            {
                return Ok(await query.Handle(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar GetByIdUser: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
