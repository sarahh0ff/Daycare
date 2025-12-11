using System.Collections.Generic;
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
            return await _context.Children.ToListAsync();
        }

        public async Task<Child?> GetByIdAsync(int id)
        {
            return await _context.Children.FindAsync(id);
        }

        public async Task AddAsync(Child entity)
        {
            await _context.Children.AddAsync(entity);
           
        }

        public void Update(Child entity)
        {
            _context.Children.Update(entity);
            
        }

        public void Delete(Child entity)
        {
            _context.Children.Remove(entity);
            
        }
    }
}
