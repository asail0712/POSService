using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using XPlan.Interface;

namespace XPlan.Controller
{
    public abstract class GenericController<TEntity, TRequest, TResponse> 
        : ControllerBase where TEntity : class, IEntity
    {
        protected readonly IService<TEntity> _service;
        protected readonly IMapper _mapper;

        public GenericController(IService<TEntity> service, IMapper mapper)
        {
            _service    = service;
            _mapper     = mapper;
        }

        // C - Create
        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TRequest requestDto)
        {
            var entity  = _mapper.Map<TEntity>(requestDto);
            var result  = await _service.CreateAsync(entity);

            return Ok(result);
        }

        // R - Read All
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var result          = await _service.GetAllAsync();
            var responseDTOs    = _mapper.Map<IEnumerable<TResponse>>(result);
            return Ok(responseDTOs);
        }

        // R - Read by Id
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            var responseDto = _mapper.Map<TResponse>(result);

            return Ok(responseDto);
        }

        // U - Update
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TRequest requestDto)
        {
            var entity      = _mapper.Map<TEntity>(requestDto);
            bool bResult    = await _service.UpdateAsync(id, entity);
            if (!bResult)
            {
                return NotFound();
            }
            return NoContent();
        }

        // D - Delete
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id)
        {
            bool bResult = await _service.DeleteAsync(id);
            if (!bResult)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
