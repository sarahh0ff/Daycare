using System.Threading.Tasks;
using Daycare.Infrastructure.Context;
using Daycare.Infrastructure.Interfaces;

namespace Daycare.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DaycareDBContext _context;

        public IChildRepository Children { get; }
        public IGuardianRepository Guardians { get; }
        public IAttendanceRepository Attendances { get; }
        public IActivityRepository Activities { get; }

        public UnitOfWork(
            DaycareDBContext context,
            IChildRepository childRepository,
            IGuardianRepository guardianRepository,
            IAttendanceRepository attendanceRepository,
            IActivityRepository activityRepository)
        {
            _context = context;
            Children = childRepository;
            Guardians = guardianRepository;
            Attendances = attendanceRepository;
            Activities = activityRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

