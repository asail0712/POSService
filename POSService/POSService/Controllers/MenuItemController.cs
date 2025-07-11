using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;
using XPlan.Interface;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuItemController : GenericController<MenuItemRequest, MenuItemResponse>
    {
        public MenuItemController(IMenuItemService service) 
            : base(service)
        {

        }
    }
}
