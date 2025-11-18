using Daycare.Domain.Entities;

namespace Daycare.Infrastructure.Interfaces
{
    public interface IAttendanceRepository
    {
        Task AddAsync(Attendance attendance);
        void Delete(Attendance attendance);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<IEnumerable<Attendance>> GetByActivityIdAsync(int activityId);
        Task<IEnumerable<Attendance>> GetByChildIdAsync(int childId);
        Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date);
        Task<Attendance?> GetByIdAsync(int id);
        void Update(Attendance attendance);
    }
}