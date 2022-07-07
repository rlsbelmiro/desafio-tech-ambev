using DevAmbev.Core.Commands.Contracts;
using DevAmbev.Core.Contracts;
using DevAmbev.Core.Contracts.Products;
using DevAmbev.Core.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevAmbev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger _logger;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromServices] ICommand<ProductRequest, ProductResponse> command, [FromBody] ProductRequest request)
        {
            try
            {
                return Ok(await command.Handle(request, User.Identity.Name));
            }
            catch(Exception ex)
            {
                var messageRequest = JsonConvert.SerializeObject(request);
                _logger.LogError($"Erro ao executar CreateProduct: {ex.Message}", messageRequest);
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> EditProduct([FromServices] ICommand<ProductUpdateRequest, ProductResponse> command, [FromBody] ProductUpdateRequest request, [FromRoute] int id)
        {
            try
            {
                request.Id = id;
                return Ok(await command.Handle(request, User.Identity.Name));
            }
            catch(Exception ex)
            {
                var messageRequest = JsonConvert.SerializeObject(request);
                _logger.LogError($"Erro ao executar EditProduct: {ex.Message}", messageRequest);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> DeleteProduct([FromServices] ICommand<int, ProductResponse> command, [FromRoute] int id)
        {
            try
            {
                return Ok(await command.Handle(id, User.Identity.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar DeleteProduct: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProductListResponse>> List([FromServices] IQuery<ProductListRequest, ProductListResponse> query)
        {
            try
            {
                return Ok(await query.Handle(new ProductListRequest()));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao executar ListProduct: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductResponse>> GetById([FromServices] IQuery<int, ProductResponse> query, [FromRoute] int id)
        {
            try
            {
                return Ok(await query.Handle(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar GetByIdProduct: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
