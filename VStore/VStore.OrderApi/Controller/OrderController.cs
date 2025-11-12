using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VStore.OrderApi.Apllication_Order.Dtos.Inputs;
using VStore.OrderApi.Apllication_Order.Dtos.Response;
using VStore.OrderApi.Domain.IService;

namespace VStore.OrderApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICRUDService<OrderResponse, OrderInput> _orderService;
        public OrderController(ICRUDService<OrderResponse, OrderInput> orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderInput orderInput)
        {
            var result = await _orderService.Create(orderInput);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(FindById), new { id = result.Data.Id }, result.Data);
            }
            return BadRequest(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAll();
            if (result.IsSuccess) 
                {
                return Ok(result.Data);
            }
            return BadRequest(result.Data);
        }

        [HttpGet("id-consumer")]
        public async Task<IActionResult> GetByConsumers(Guid id)
        {
            var result = await _orderService.GetByListConsumer(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _orderService.FindById(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.Delete(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.Message);
        }
    }
}
