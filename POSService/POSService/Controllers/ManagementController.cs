using Microsoft.AspNetCore.Mvc;

using Common.DTO;
using Service.Interface;

using XPlan.Controller;
using XPlan.Interface;

namespace POSService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : GenericController<StaffDataRequest, StaffDataResponse>
    {
        public ManagementController(IManagementService service)
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

        // R - Read by Id
        [NonAction]
        public override async Task<IActionResult> GetById(string id)
        {
            return await base.GetById(id);
        }

        // U - Update
        [NonAction]
        public override async Task<IActionResult> Update(string id, [FromBody] StaffDataRequest requestDto)
        {
            return await base.Update(id, requestDto);
        }

    }
}
