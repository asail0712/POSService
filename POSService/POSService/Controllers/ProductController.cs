using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : GenericController<ProductInfoRequest, ProductInfoResponse, IProductService>
    {
        public ProductController(IProductService service)
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
