using System.Threading.Tasks;

namespace Daycare.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IChildRepository Children { get; }
        IGuardianRepository Guardians { get; }
        IAttendanceRepository Attendances { get; }
        IActivityRepository Activities { get; }

        Task<int> SaveChangesAsync();
    }
}
