using AutoMapper;
using Daycare.Application.DTOs;
using Daycare.Domain.Entities;
using Daycare.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daycare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly IChildRepository _childRepository;
        private readonly IMapper _mapper;

        public ChildController(IChildRepository childRepository, IMapper mapper)
        {
            _childRepository = childRepository;
            _mapper = mapper;
        }

        // ================================
        // GET ALL - api/child
        // ================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var children = await _childRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ChildDto>>(children);
            return Ok(result);
        }

        // ================================
        // GET BY ID - api/child/{id}
        // ================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var child = await _childRepository.GetByIdAsync(id);

            if (child == null)
                return NotFound(new { message = $"Child with ID {id} not found." });

            var result = _mapper.Map<ChildDto>(child);
            return Ok(result);
        }

        // ================================
        // CREATE - api/child
        // ================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChildDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newChild = _mapper.Map<Child>(dto);

            await _childRepository.AddAsync(newChild);

            var result = _mapper.Map<ChildDto>(newChild);

            return CreatedAtAction(nameof(GetById), new { id = newChild.Id }, result);
        }


        // ================================
        // UPDATE - api/child/{id}
        // ================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChildDto dto)
        {
            var child = await _childRepository.GetByIdAsync(id);

            if (child == null)
                return NotFound(new { message = $"Child with ID {id} not found." });

            _mapper.Map(dto, child);

            _childRepository.Update(child);

            var result = _mapper.Map<ChildDto>(child);

            return Ok(result);
        }


        // ================================
        // DELETE - api/child/{id}
        // ================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var child = await _childRepository.GetByIdAsync(id);

            if (child == null)
                return NotFound(new { message = $"Child with ID {id} not found." });

            _childRepository.Delete(child);

            return NoContent();
        }
    }
}
