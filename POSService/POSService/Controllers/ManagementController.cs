using Common.DTO;
using Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using XPlan.Controller;
using XPlan.Interface;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : GenericController<StaffDataRequest, StaffDataResponse, IManagementService>
    {
        public ManagementController(IManagementService service)
            : base(service)
        {

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var bResult = await _service.Login(request);

            return Ok(bResult);
        }
    }
}
