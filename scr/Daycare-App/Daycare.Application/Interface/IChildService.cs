using Daycare.Application.DTOs;

namespace Daycare.Application.Interfaces
{
    public interface IChildService
    {
        Task<IEnumerable<ChildDto>> GetAllAsync();
        Task<ChildDto?> GetByIdAsync(int id);
        Task<ChildDto> CreateAsync(ChildDto dto);
        Task<bool> UpdateAsync(int id, ChildDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
