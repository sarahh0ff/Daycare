
using Daycare.Application.DTOs;
using Daycare.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daycore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   
    public class GuardiansController : ControllerBase
    {
        private readonly IGuardianService _guardianService;

        public GuardiansController(IGuardianService guardianService)
        {
            _guardianService = guardianService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuardianDto>>> GetAll()
        {
            var result = await _guardianService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GuardianDto>> GetById(int id)
        {
            var guardian = await _guardianService.GetByIdAsync(id);
            if (guardian == null) return NotFound();
            return Ok(guardian);
        }

        [HttpPost]
        public async Task<ActionResult<GuardianDto>> Create([FromBody] GuardianDto dto)
        {
            var created = await _guardianService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GuardianDto dto)
        {
            var ok = await _guardianService.UpdateAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _guardianService.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
