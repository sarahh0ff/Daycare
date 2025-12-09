
using AutoMapper;
using Daycare.Application.DTOs;
using Daycare.Application.Interfaces;
using Daycare.Domain.Entities;
using Daycare.Infrastructure.Interfaces;

namespace Daycare.Application.Services
{
    public class GuardianService : IGuardianService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GuardianService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GuardianDto>> GetAllAsync()
        {
            var guardians = await _unitOfWork.Guardians.GetAllAsync();
            return _mapper.Map<IEnumerable<GuardianDto>>(guardians);
        }

        public async Task<GuardianDto?> GetByIdAsync(int id)
        {
            var guardian = await _unitOfWork.Guardians.GetByIdAsync(id);
            return guardian == null ? null : _mapper.Map<GuardianDto>(guardian);
        }

        public async Task<GuardianDto> CreateAsync(GuardianDto dto)
        {
            var entity = _mapper.Map<Guardian>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;

            await _unitOfWork.Guardians.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GuardianDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, GuardianDto dto)
        {
            var existing = await _unitOfWork.Guardians.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Guardians.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Guardians.GetByIdAsync(id);
            if (existing == null) return false;

            existing.IsDeleted = true;
            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Guardians.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
