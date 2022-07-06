using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Orders;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevAmbev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse>> CreateCustomer([FromServices] IQuery<LoginUserRequest, LoginUserResponse> command, [FromBody] LoginUserRequest request)
        {
            try
            {
                var result = await command.Handle(request);
                result.Token = TokenService.GenerateToken(new UserResponse()
                {
                    Name = result.Name,
                    Email = result.Email,
                    Id = result.Id
                });
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

    }
}
