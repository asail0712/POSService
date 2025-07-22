using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using XPlan.Service;

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
        public async Task<IActionResult> Create([FromBody] TRequest requestDto)
        {
            return Ok(await OnCreate(requestDto));
        }

        // R - Read All
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await OnGetAll());
        }

        // R - Read by Id
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await OnGet(key));
        }

        // U - Update
        [HttpPut("{key}")]
        public async Task<IActionResult> Update(string key, [FromBody] TRequest requestDto)
        {
            await OnUpdate(key, requestDto);

            return Ok();
        }

        // D - Delete
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            await OnDelete(key);

            return Ok();
        }

        protected virtual async Task<TResponse> OnCreate(TRequest request)
        {
            return await _service.CreateAsync(request);
        }
        protected virtual async Task<TResponse> OnGet(string key)
        {
            return await _service.GetAsync(key);
        }
        protected virtual async Task<List<TResponse>> OnGetAll()
        {
            return await _service.GetAllAsync();
        }
        protected virtual async Task OnUpdate(string key, TRequest request)
        {
            await _service.UpdateAsync(key, request);
        }
        protected virtual async Task OnDelete(string key)
        {
            await _service.DeleteAsync(key);
        }
    }
}
