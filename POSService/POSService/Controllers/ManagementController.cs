using Common.DTO.Management;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using XPlan.Controller;

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

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var bResult = await _service.ChangePassword(request);

            return Ok(bResult);
        }

        // U - Update
        [HttpPut("ChangeData/{key}")]
        public async Task<IActionResult> ChangeData(string key, [FromBody] ChangeStaffDataRequest requestDto)
        {
            StaffDataRequest staffDataRequest = new StaffDataRequest
            {
                Account   = requestDto.Account,
                Name      = requestDto.Name,
                IsActive  = requestDto.IsActive
            };

            var result = await base.Update(key, staffDataRequest);

            return Ok(result);
        }
    }
}
