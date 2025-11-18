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
    public class ChildRepository : IChildRepository
    {
        private readonly DaycareDBContext _context;

        public ChildRepository(DaycareDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Child>> GetAllAsync()
        {
            return await _context.Children
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Child?> GetByIdAsync(int id)
        {
            return await _context.Children
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task AddAsync(Child child)
        {
            await _context.Children.AddAsync(child);
        }

        public void Update(Child child)
        {
            child.UpdatedAt = DateTime.UtcNow;
            _context.Children.Update(child);
        }

        public void Delete(Child child)
        {
            child.MarkAsDeleted();
            _context.Children.Update(child);
        }

        public async Task<IEnumerable<Child>> GetByGuardianIdAsync(int guardianId)
        {
            return await _context.Children
                .Where(c => c.GuardianId == guardianId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Children.AnyAsync(c => c.Id == id && !c.IsDeleted);
        }
    }
}
