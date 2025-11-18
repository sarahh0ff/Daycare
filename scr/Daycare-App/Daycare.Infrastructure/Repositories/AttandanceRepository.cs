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
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly DaycareDBContext _context;

        public AttendanceRepository(DaycareDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .Include(a => a.Child)
                .Include(a => a.Activity)
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Child)
                .Include(a => a.Activity)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task AddAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
        }

        public void Update(Attendance attendance)
        {
            attendance.UpdatedAt = DateTime.UtcNow;
            _context.Attendances.Update(attendance);
        }

        public void Delete(Attendance attendance)
        {
            attendance.MarkAsDeleted();
            _context.Attendances.Update(attendance);
        }

        public async Task<IEnumerable<Attendance>> GetByChildIdAsync(int childId)
        {
            return await _context.Attendances
                .Where(a => a.ChildId == childId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByActivityIdAsync(int activityId)
        {
            return await _context.Attendances
                .Where(a => a.ActivityId == activityId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date)
        {
            return await _context.Attendances
                .Where(a => a.Date.Date == date.Date && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Attendances.AnyAsync(a => a.Id == id && !a.IsDeleted);
        }
    }
}
