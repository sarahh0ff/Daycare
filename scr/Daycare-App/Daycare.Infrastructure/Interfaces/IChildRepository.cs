using Daycare.Domain.Entities;

namespace Daycare.Infrastructure.Interfaces
{
    public interface IChildRepository
    {
        Task AddAsync(Child child);
        void Delete(Child child);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Child>> GetAllAsync();
        Task<IEnumerable<Child>> GetByGuardianIdAsync(int guardianId);
        Task<Child?> GetByIdAsync(int id);
        void Update(Child child);
    }
}