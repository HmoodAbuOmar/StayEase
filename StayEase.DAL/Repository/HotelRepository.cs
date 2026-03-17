using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Data;
using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.Include(c => c.Translations).ToListAsync();
        }

        public async Task<Hotel> CreateAsync(Hotel request)
        {
            await _context.Hotels.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
        public async Task<Hotel?> FindByIdAsync(int id)
        {
            return await _context.Hotels.Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Hotel> UpdateAsync(Hotel request)
        {
            _context.Hotels.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task DeleteAsync(Hotel request)
        {
            _context.Hotels.Remove(request);
            await _context.SaveChangesAsync();

        }


        public async Task<List<Hotel>?> GetActiveHotelAsync()
        {
            return await _context.Hotels
                .Include(c => c.Translations)
                .Where(c => c.IsActive)
                .ToListAsync();
        }
    }
}