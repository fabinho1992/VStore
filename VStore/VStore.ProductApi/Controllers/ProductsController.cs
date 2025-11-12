using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VStore.ProductApi.Application.Dtos.Inputs;
using VStore.ProductApi.Application.Dtos.Responses;
using VStore.ProductApi.Domain.IService;

namespace VStore.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICRUDService<ProductResponse, ProductInput> _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ICRUDService<ProductResponse, ProductInput> service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductInput input)
        {
            try
            {
                var result = await _service.Create(input);
                return CreatedAtAction(nameof(Create), new { id = result.Data.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("products-ids")]
        public async Task<IActionResult> GetProductsByIds([FromQuery] string ids)
        {
            try
            { 
                var result = await _service.GetProductsOrder(ids);
                if (result.Data == null || !result.Data.Any())
                {
                    return NotFound(result.Message);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products by IDs.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var result = await _service.FindById(id);
                if (result == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var results = await _service.GetAll();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> FindByText([FromQuery] string query)
        {
            try
            {
                var results = await _service.FindByText(query);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] ProductInput input)
        {
            try
            {
                var result = await _service.Update(id, input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (!result.IsSuccess)
                {
                    return NotFound(result.Message);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the product.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
