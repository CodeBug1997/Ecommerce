using Ecommerce.Service.Dtos.OrderDtos;
using Ecommerce.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var result = await _orderService.GetOrderAsync(id);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequestDto request)
        {
            var result = await _orderService.CreateOrderAsync(request);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] long id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            return Ok(result);
        }
    }
}