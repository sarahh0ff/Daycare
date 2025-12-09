using Daycare.Application.DTOs;
using Daycare.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daycore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]          
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivitiesController(IActivityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivityDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActivityDto dto)
        {
            var result = await _service.CreateAsync(dto);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ActivityDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

