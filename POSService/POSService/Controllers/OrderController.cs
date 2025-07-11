using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;
using XPlan.Interface;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : GenericController<OrderDetailRequest, OrderDetailResponse>
    {
        public OrderController(IOrderService service)
            : base(service)
        {

        }

        // R - Read All
        [NonAction]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }
    }
}
