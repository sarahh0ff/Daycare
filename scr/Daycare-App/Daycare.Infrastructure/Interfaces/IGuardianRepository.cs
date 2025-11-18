using Daycare.Domain.Entities;

namespace Daycare.Infrastructure.Interfaces
{
    public interface IGuardianRepository
    {
        Task AddAsync(Guardian guardian);
        void Delete(Guardian guardian);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Guardian>> GetAllAsync();
        Task<Guardian?> GetByIdAsync(int id);
        Task<IEnumerable<Guardian>> GetEmergencyContactsAsync();
        void Update(Guardian guardian);
    }
}