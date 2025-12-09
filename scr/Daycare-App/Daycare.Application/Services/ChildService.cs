using Daycare.Infrastructure.Interfaces;
using Daycare.Application.Interfaces;
using Daycare.Application.DTOs;
using Daycare.Domain.Entities;
using AutoMapper;

namespace Daycare.Application.Services
{
    public class ChildService : IChildService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChildService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChildDto>> GetAllAsync()
        {
            var children = await _unitOfWork.Children.GetAllAsync();
            return _mapper.Map<IEnumerable<ChildDto>>(children);
        }

        public async Task<ChildDto?> GetByIdAsync(int id)
        {
            var child = await _unitOfWork.Children.GetByIdAsync(id);
            if (child == null) return null;

            return _mapper.Map<ChildDto>(child);
        }

        public async Task<ChildDto> CreateAsync(ChildDto dto)
        {
            var entity = _mapper.Map<Child>(dto);

            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            entity.IsDeleted = false;

            await _unitOfWork.Children.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();   

            return _mapper.Map<ChildDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ChildDto dto)
        {
            var existing = await _unitOfWork.Children.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Children.Update(existing);
            await _unitOfWork.SaveChangesAsync();   

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _unitOfWork.Children.GetByIdAsync(id);
            if (existing == null) return false;

            existing.IsDeleted = true;
            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Children.Update(existing);
            await _unitOfWork.SaveChangesAsync();   

            return true;
        }
    }
}
