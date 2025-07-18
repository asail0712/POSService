using Microsoft.AspNetCore.Mvc;
using Service.Interface;

using Common.DTO.Dish;

using XPlan.Controller;

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

        [NonAction]
        [HttpGet("GetAllBrief")]
        public async Task<IActionResult> GetAllBriefAsync()
        {
            var result = await _service.GetAllBriefAsync();

            return Ok(result);
        }

        [NonAction]
        [HttpGet("{key}/GetBrief")]
        public async Task<IActionResult> GetBriefAsync(string key)
        {
            var result = await _service.GetBriefAsync(key);

            return Ok(result);
        }
    }
}
