using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;
using XPlan.Utility.Error;

namespace XPlan.Controller
{
    public abstract class GenericController<TRequest, TResponse, TService> 
        : ControllerBase where TService : IService<TRequest, TResponse>
    {
        protected readonly TService _service;

        public GenericController(TService service)
        {
            _service = service;
        }

        // C - Create
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TRequest requestDto)
        {
            await _service.CreateAsync(requestDto);

            return Ok();
        }

        // R - Read All
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        // R - Read by Id
        [HttpGet("{key}")]
        public virtual async Task<IActionResult> GetAsync(string key)
        {
            var result = await _service.GetAsync(key);

            return Ok(result);
        }

        // R - Read by Id
        // /api/yourcontroller/by-time?startTime=2025-07-01&endTime=2025-07-15
        [HttpGet("by-time")]
        public virtual async Task<IActionResult> GetByTimeAsync([FromQuery] DateTime? startTime = null, [FromQuery] DateTime? endTime = null)
        {
            var result = await _service.GetByTimeAsync(startTime, endTime);

            return Ok(result);
        }

        // U - Update
        [HttpPut("{key}")]
        public virtual async Task<IActionResult> Update(string key, [FromBody] TRequest requestDto)
        {
            bool bResult = await _service.UpdateAsync(key, requestDto);
            
            return NoContent();
        }

        // D - Delete
        [HttpDelete("{key}")]
        public virtual async Task<IActionResult> Delete(string key)
        {
            bool bDeleted = await _service.DeleteAsync(key);

            return NoContent();
        }
    }
}
