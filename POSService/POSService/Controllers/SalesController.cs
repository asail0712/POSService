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

        [HttpPost("GetTotalSalesAmount")]
        public async Task<IActionResult> GetTotalSalesAmount(SoldItemRequest request)
        {
            var result = await _service.GetTotalSalesAmount(request);

            return Ok(result);
        }

        [HttpPost("{id}/GeConsumptionCount")]
        public async Task<IActionResult> GeConsumptionCount(string id)
        {
            // ED TODO

            return Ok();
        }

        /*********************************
         * 隱藏的API
         * ******************************/

        // R - Read All
        [NonAction]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }

        // R - Read by Id
        [NonAction]
        public override async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        // U - Update
        [NonAction]
        public override async Task<IActionResult> Update(string id, [FromBody] SoldItemRequest requestDto)
        {
            return await Update(id, requestDto);
        }

        // D - Delete
        [NonAction]
        public override async Task<IActionResult> Delete(string id)
        {
            return await Delete(id);
        }
    }
}

