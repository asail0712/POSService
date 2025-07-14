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
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);

            return Ok(result);
        }

        // U - Update
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TRequest requestDto)
        {
            bool bResult = await _service.UpdateAsync(id, requestDto);
            
            return NoContent();
        }

        // D - Delete
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            bool bDeleted = await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
