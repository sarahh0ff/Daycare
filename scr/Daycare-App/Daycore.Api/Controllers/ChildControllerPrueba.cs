using Daycare.Application.DTOs;
using Daycare.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daycore.Api.Controllers   
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildrenController(IChildService childService)
        {
            _childService = childService;
        }

        // GET: api/children
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChildDto>>> GetAll()
        {
            var result = await _childService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/children/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ChildDto>> GetById(int id)
        {
            var child = await _childService.GetByIdAsync(id);
            if (child == null) return NotFound();
            return Ok(child);
        }

        // POST: api/children
        [HttpPost]
        public async Task<ActionResult<ChildDto>> Create([FromBody] ChildDto dto)
        {
            var created = await _childService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/children/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChildDto dto)
        {
            var success = await _childService.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/children/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _childService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
