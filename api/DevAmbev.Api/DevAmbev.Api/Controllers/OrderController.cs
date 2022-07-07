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
    public class OrderController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> CreateOrder([FromServices] ICommand<OrderRequest, OrderResponse> command, [FromBody] OrderRequest request)
        {
            try
            {
                return Ok(await command.Handle(request, User.Identity.Name));
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<OrderListResponse>> ListOrders([FromServices] IQuery<OrderListRequest, OrderListResponse> query)
        {
            try
            {
                return Ok(await query.Handle(new OrderListRequest()));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderListResponse>> GetById([FromServices] IQuery<int, OrderResponse> query, [FromRoute] int id)
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
