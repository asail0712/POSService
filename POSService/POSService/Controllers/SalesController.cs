using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : GenericController<SoldItemRequest, SoldItemResponse>
    {
        public SalesController(ISalesService service)
            : base(service)
        {

        }

        // R - Read All
        [NonAction]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
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

