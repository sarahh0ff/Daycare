using System.Collections.Generic;
using System.Threading.Tasks;
using Daycare.Domain.Entities;

namespace Daycare.Infrastructure.Interfaces
{
    public interface IChildRepository
    {
        Task<IEnumerable<Child>> GetAllAsync();
        Task<Child?> GetByIdAsync(int id);
        Task AddAsync(Child entity);
        void Update(Child entity);
        void Delete(Child entity);
    }
}
