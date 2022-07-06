using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Customers;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace DevAmbev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromServices] ICommand<CustomerRequest, CustomerResponse> command, [FromBody] CustomerRequest request)
        {
            try
            {
                return Ok(await command.Handle(request));
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<CustomerResponse>> EditCustomer([FromServices] ICommand<CustomerUpdateRequest, CustomerResponse> command, [FromBody] CustomerUpdateRequest request, [FromRoute] int id)
        {
            try
            {
                request.Id = id;
                return Ok(await command.Handle(request));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<BaseResponse>> DeleteProduct([FromServices] ICommand<int, BaseResponse> command, [FromRoute] int id)
        {
            try
            {
                return Ok(await command.Handle(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<CustomerListResponse>> List([FromServices] IQuery<CustomerListRequest, CustomerListResponse> query)
        {
            try
            {
                return Ok(await query.Handle(new CustomerListRequest()));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetById([FromServices] IQuery<int, CustomerResponse> query, [FromRoute] int id)
        {
            try
            {
                return Ok(await query.Handle(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
