using AutoMapper;
using Daycare.Application.DTOs;
using Daycare.Application.Interfaces;
using Daycare.Infrastructure.Interfaces;
using Daycare.Domain.Entities;   


namespace Daycare.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityDto>> GetAllAsync()
        {
            var data = await _unitOfWork.Activities.GetAllAsync();
            return _mapper.Map<IEnumerable<ActivityDto>>(data);
        }

        public async Task<ActivityDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Activities.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ActivityDto>(entity);
        }

        public async Task<ActivityDto> CreateAsync(ActivityDto dto)
        {
            var entity = _mapper.Map<Activity>(dto);

            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Activities.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ActivityDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ActivityDto dto)
        {
            var existing = await _unitOfWork.Activities.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Activities.Update(existing);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Activities.GetByIdAsync(id);
            if (entity == null) return false;

            entity.MarkAsDeleted();
            _unitOfWork.Activities.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
