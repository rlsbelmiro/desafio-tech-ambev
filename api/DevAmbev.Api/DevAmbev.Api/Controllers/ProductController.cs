using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using Microsoft.AspNetCore.Mvc;

namespace DevAmbev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromServices] ICommand<ProductRequest, ProductResponse> command, [FromBody] ProductRequest request)
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
        public async Task<ActionResult<ProductResponse>> EditProduct([FromServices] ICommand<ProductUpdateRequest, ProductResponse> command, [FromBody] ProductUpdateRequest request, [FromRoute] int id)
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
        public async Task<ActionResult<ProductListResponse>> List([FromServices] IQuery<ProductListRequest, ProductListResponse> query)
        {
            try
            {
                return Ok(await query.Handle(new ProductListRequest()));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductResponse>> GetById([FromServices] IQuery<int, ProductResponse> query, [FromRoute] int id)
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
