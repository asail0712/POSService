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
    }
}
