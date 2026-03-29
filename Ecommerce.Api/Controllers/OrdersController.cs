using Ecommerce.Service.Dtos;
using Ecommerce.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            var result = await _orderService.CreateOrderAsync(request);
            return Ok(result);
        }
    }
}