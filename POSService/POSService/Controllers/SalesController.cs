using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : GenericController<SoldItemRequest, SoldItemResponse, ISalesService>
    {
        public SalesController(ISalesService service)
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

        /*********************************
         * 隱藏的API
         * ******************************/
        [NonAction]
        public override async Task<IActionResult> CreateAsync([FromBody] SoldItemRequest requestDto)
        {
            await _service.CreateAsync(requestDto);

            return Ok();
        }

        // R - Read All
        [NonAction]
        public override async Task<IActionResult> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        // R - Read by Id
        [NonAction]
        public override async Task<IActionResult> GetAsync(string id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        // U - Update
        [NonAction]
        public override async Task<IActionResult> UpdateAsync(string id, [FromBody] SoldItemRequest requestDto)
        {
            return await UpdateAsync(id, requestDto);
        }

        // D - Delete
        [NonAction]
        public override async Task<IActionResult> DeleteAsync(string id)
        {
            return await DeleteAsync(id);
        }
    }
}

