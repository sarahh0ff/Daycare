
using Daycare.Application.DTOs;

namespace Daycare.Application.Interfaces
{
    public interface IGuardianService
    {
        Task<IEnumerable<GuardianDto>> GetAllAsync();
        Task<GuardianDto?> GetByIdAsync(int id);
        Task<GuardianDto> CreateAsync(GuardianDto dto);
        Task<bool> UpdateAsync(int id, GuardianDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
