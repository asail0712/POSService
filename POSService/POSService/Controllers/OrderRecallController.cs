using Microsoft.AspNetCore.Mvc;
using Service.Interface;

using XPlan.Controller;
using Common.DTO.OrderRecall;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderRecallController : GenericController<OrderRecallRequest, OrderRecallResponse, IOrderRecallService>
    {
        public OrderRecallController(IOrderRecallService service)
            : base(service)
        {

        }

        [HttpPost("GetSalesByTime")]
        public async Task<IActionResult> GetSalesByTime([FromBody]TimeRangeSalesRequest request)
        {
            var result = await _service.GetSalesByTime(request);

            return Ok(result);
        }

        [HttpPost("GetProductSalesByTime")]
        public async Task<IActionResult> GetProductSalesByTime([FromBody]TimeRangeProductSalesRequest request)
        {
            var result = await _service.GetProductSalesByTime(request);

            return Ok(result);
        }
    }
}

