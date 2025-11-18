using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daycare.Domain.Entities;
using Daycare.Infrastructure.Context;
using Daycare.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Daycare.Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly DaycareDBContext _context;

        public ActivityRepository(DaycareDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            return await _context.Activities
                .Include(a => a.Attendances)
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Attendances)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task AddAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
        }

        public void Update(Activity activity)
        {
            activity.UpdatedAt = DateTime.UtcNow;
            _context.Activities.Update(activity);
        }

        public void Delete(Activity activity)
        {
            activity.MarkAsDeleted();
            _context.Activities.Update(activity);
        }

        public async Task<IEnumerable<Activity>> GetByDateAsync(DateTime date)
        {
            return await _context.Activities
                .Where(a => a.StartTime.Date == date.Date && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Activities.AnyAsync(a => a.Id == id && !a.IsDeleted);
        }
    }
}
