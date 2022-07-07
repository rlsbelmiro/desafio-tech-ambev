using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Orders;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevAmbev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger _logger;
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse>> Login([FromServices] IQuery<LoginUserRequest, LoginUserResponse> command, [FromBody] LoginUserRequest request)
        {
            try
            {
                var result = await command.Handle(request);
                if(result.Success)
                {
                    result.Token = TokenService.GenerateToken(new UserResponse()
                    {
                        Name = result.Name,
                        Email = result.Email,
                        Id = result.Id
                    });
                    return Ok(result);
                }
                else
                {
                    return StatusCode(401, "Login ou Senha inválidos");
                }
                
            }
            catch(Exception ex)
            {
                var messageRequest = JsonConvert.SerializeObject(request);
                _logger.LogError($"Erro ao executar Login: {ex.Message}", messageRequest);
                return StatusCode(500,ex.Message);
            }
        }

    }
}
