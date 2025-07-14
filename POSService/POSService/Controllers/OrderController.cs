using Common.DTO;
using Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using XPlan.Controller;
using XPlan.Interface;

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

        [HttpPost("{orderId}/Status")]
        public async Task<IActionResult> ModifyOrderStatus(string orderId, [FromBody] OrderStatus status)
        {
            var bResult = await ModifyOrderStatus(orderId, status);

            return Ok(bResult);
        }

        /*********************************
         * 隱藏的API
         * ******************************/
        [NonAction]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }

        // R - Read by Id
        [NonAction]
        public override async Task<IActionResult> GetById(string id)
        {
            return await base.GetById(id);
        }

        // U - Update
        [NonAction]
        public override async Task<IActionResult> Update(string id, [FromBody] OrderDetailRequest requestDto)
        {
            return await base.Update(id, requestDto);
        }
    }
}
