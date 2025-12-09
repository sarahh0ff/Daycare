using Daycare.Application.DTOs;

namespace Daycare.Application.Interfaces
{
    public interface IActivityService
    {
        Task<IEnumerable<ActivityDto>> GetAllAsync();
        Task<ActivityDto?> GetByIdAsync(int id);
        Task<ActivityDto> CreateAsync(ActivityDto dto);
        Task<bool> UpdateAsync(int id, ActivityDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
