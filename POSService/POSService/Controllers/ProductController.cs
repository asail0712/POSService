using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : GenericController<ProductPackageRequest, ProductPackageResponse, IProductService>
    {
        public ProductController(IProductService service)
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
