using Daycare.Application.DTOs;

namespace Daycare.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAllAsync();
        Task<AttendanceDto?> GetByIdAsync(int id);

        Task<IEnumerable<AttendanceDto>> GetByChildIdAsync(int childId);
        Task<IEnumerable<AttendanceDto>> GetByActivityIdAsync(int activityId);
        Task<IEnumerable<AttendanceDto>> GetByDateAsync(DateTime date);

        Task<AttendanceDto> CreateAsync(AttendanceDto dto);
        Task<bool> UpdateAsync(int id, AttendanceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
