using Daycare.Application.DTOs;
using Daycare.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daycore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]         
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _service;

        public AttendancesController(IAttendanceService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AttendanceDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("by-child/{childId:int}")]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetByChild(int childId)
        {
            var result = await _service.GetByChildIdAsync(childId);
            return Ok(result);
        }


        [HttpGet("by-activity/{activityId:int}")]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetByActivity(int activityId)
        {
            var result = await _service.GetByActivityIdAsync(activityId);
            return Ok(result);
        }

      
        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetByDate([FromQuery] DateTime date)
        {
            var result = await _service.GetByDateAsync(date);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AttendanceDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AttendanceDto dto)
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
