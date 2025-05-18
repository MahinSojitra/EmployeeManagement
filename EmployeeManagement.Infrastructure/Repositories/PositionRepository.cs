using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly AppDbContext _context;

        public PositionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<Position?> GetByIdAsync(Guid id)
        {
            return await _context.Positions.FindAsync(id);
        }

        public async Task AddAsync(Position position)
        {
            await _context.Positions.AddAsync(position);
        }

        public async Task<bool> UpdateAsync(Position position)
        {
            var exists = await _context.Positions.AnyAsync(p => p.Id == position.Id);
            if (!exists) return false;

            _context.Positions.Update(position);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null) return false;

            _context.Positions.Remove(position);
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Positions.AnyAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}