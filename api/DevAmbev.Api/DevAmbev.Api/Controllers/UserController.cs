using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DevAmbev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromServices] ICommand<UserRequest, UserResponse> command, [FromBody] UserRequest userRequest)
        {
            try
            {
                return Ok(await command.Handle(userRequest));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
