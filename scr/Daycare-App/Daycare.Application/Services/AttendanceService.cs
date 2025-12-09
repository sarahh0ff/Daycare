using AutoMapper;
using Daycare.Application.DTOs;
using Daycare.Application.Interfaces;
using Daycare.Domain.Entities;
using Daycare.Infrastructure.Interfaces;

namespace Daycare.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Attendances.GetAllAsync();
            return _mapper.Map<IEnumerable<AttendanceDto>>(list);
        }

        public async Task<AttendanceDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Attendances.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<AttendanceDto>(entity);
        }

        public async Task<IEnumerable<AttendanceDto>> GetByChildIdAsync(int childId)
        {
            var list = await _unitOfWork.Attendances.GetByChildIdAsync(childId);
            return _mapper.Map<IEnumerable<AttendanceDto>>(list);
        }

        public async Task<IEnumerable<AttendanceDto>> GetByActivityIdAsync(int activityId)
        {
            var list = await _unitOfWork.Attendances.GetByActivityIdAsync(activityId);
            return _mapper.Map<IEnumerable<AttendanceDto>>(list);
        }

        public async Task<IEnumerable<AttendanceDto>> GetByDateAsync(DateTime date)
        {
            var list = await _unitOfWork.Attendances.GetByDateAsync(date);
            return _mapper.Map<IEnumerable<AttendanceDto>>(list);
        }

        public async Task<AttendanceDto> CreateAsync(AttendanceDto dto)
        {
            var entity = _mapper.Map<Attendance>(dto);

            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _unitOfWork.Attendances.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AttendanceDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, AttendanceDto dto)
        {
            var existing = await _unitOfWork.Attendances.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Attendances.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Attendances.GetByIdAsync(id);
            if (existing == null) return false;

            existing.IsDeleted = true;
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Attendances.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
