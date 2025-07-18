﻿using Common.DTO;
using Common.Entities;
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
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var bResult = await _service.ChangePassword(request);

            return Ok(bResult);
        }

        [HttpPost]
        //[Authorize]
        public override async Task<IActionResult> CreateAsync([FromBody] StaffDataRequest requestDto)
        {
            await base.CreateAsync(requestDto);

            return Ok();
        }

        // R - Read All
        [HttpGet]
        //[Authorize]
        public override async Task<IActionResult> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        // R - Read by Id
        [HttpGet("{key}")]
        //[Authorize]
        public override async Task<IActionResult> GetAsync(string key)
        {
            return await base.GetAsync(key);
        }

        // U - Update
        [HttpPut("ChangeData/{key}")]
        [Authorize]
        public async Task<IActionResult> ChangeData(string key, [FromBody] ChangeStaffDataRequest requestDto)
        {
            StaffDataRequest staffDataRequest = new StaffDataRequest
            {
                Account   = requestDto.Account,
                Name      = requestDto.Name,
                IsActive  = requestDto.IsActive
            };
            return await base.UpdateAsync(key, staffDataRequest);
        }

        // D - Delete
        [HttpDelete("{key}")]
        [Authorize]
        public override async Task<IActionResult> DeleteAsync(string key)
        { 
            return await base.DeleteAsync(key);
        }
    }
}
