using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;
using XPlan.Interface;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DishItemController : GenericController<DishItemRequest, DishItemResponse, IDishItemService>
    {
        public DishItemController(IDishItemService service) 
            : base(service)
        {

        }

        [HttpGet("{key}/GetBrief")]
        public async Task<IActionResult> GetBriefAsync(string key)
        {
            var result = await _service.GetBriefAsync(key);

            return Ok(result);
        }
    }
}
