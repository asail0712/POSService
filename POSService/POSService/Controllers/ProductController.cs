using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : GenericController<ProductInfoRequest, ProductInfoResponse>
    {
        public ProductController(IProductService service)
            : base(service)
        {

        }

        // R - Read All
        [NonAction]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }
    }
}
