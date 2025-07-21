using Common.DTO.Order;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using XPlan.Controller;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : GenericController<OrderDetailRequest, OrderDetailResponse, IOrderService>
    {
        public OrderController(IOrderService service)
            : base(service)
        {

        }

        [HttpPut("{orderId}/Status")]
        public async Task<IActionResult> ModifyOrderStatus(string orderId, [FromBody] OrderStatus status)
        {
            var bResult = await _service.ModifyOrderStatus(orderId, status);

            return Ok(bResult);
        }
    }
}
