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


        /*********************************
         * 隱藏的API
         * ******************************/
        // R - Read All
        [NonAction]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }
    }
}
