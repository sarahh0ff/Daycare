using Daycare.Domain.Entities;

namespace Daycare.Infrastructure.Interfaces
{
    public interface IActivityRepository
    {
        Task AddAsync(Activity activity);
        void Delete(Activity activity);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Activity>> GetAllAsync();
        Task<IEnumerable<Activity>> GetByDateAsync(DateTime date);
        Task<Activity?> GetByIdAsync(int id);
        void Update(Activity activity);
    }
}