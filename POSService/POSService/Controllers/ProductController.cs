using Microsoft.AspNetCore.Mvc;
using Service.Interface;

using XPlan.Controller;
using Common.DTO.Product;

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

        [HttpGet("GetAllBrief")]
        public async Task<IActionResult> GetAllBrief()
        {
            var result = await _service.GetAllBriefAsync();

            return Ok(result);
        }

        [HttpGet("{key}/GetBrief")]
        public async Task<IActionResult> GetBrief(string key)
        {
            var result = await _service.GetBriefAsync(key);

            return Ok(result);
        }

        [HttpPut("{key}/ReduceStock/{numOfReduce}")]
        public async Task<IActionResult> ReduceStock(string key, int numOfReduce)
        {
            await _service.ReduceStock(key, numOfReduce);

            return Ok();
        }
    }
}
