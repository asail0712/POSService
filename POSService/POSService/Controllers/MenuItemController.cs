using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;
using XPlan.Interface;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuItemController : GenericController<MenuItemRequest, MenuItemResponse, IMenuItemService>
    {
        public MenuItemController(IMenuItemService service) 
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
