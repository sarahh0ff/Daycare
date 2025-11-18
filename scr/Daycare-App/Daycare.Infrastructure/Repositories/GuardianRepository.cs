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
    public class GuardianRepository : IGuardianRepository
    {
        private readonly DaycareDBContext _context;

        public GuardianRepository(DaycareDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Guardian>> GetAllAsync()
        {
            return await _context.Guardians
                .Where(g => !g.IsDeleted)
                .ToListAsync();
        }

        public async Task<Guardian?> GetByIdAsync(int id)
        {
            return await _context.Guardians
                .Include(g => g.Children) 
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
        }

        public async Task AddAsync(Guardian guardian)
        {
            await _context.Guardians.AddAsync(guardian);
        }

        public void Update(Guardian guardian)
        {
            guardian.UpdatedAt = DateTime.UtcNow;
            _context.Guardians.Update(guardian);
        }

        public void Delete(Guardian guardian)
        {
            guardian.MarkAsDeleted();
            _context.Guardians.Update(guardian);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Guardians.AnyAsync(g => g.Id == id && !g.IsDeleted);
        }

        public async Task<IEnumerable<Guardian>> GetEmergencyContactsAsync()
        {
            return await _context.Guardians
                .Where(g => g.IsEmergencyContact && !g.IsDeleted)
                .ToListAsync();
        }
    }
}

