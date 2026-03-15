using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Data;
using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<RoomType>> GetAllAsync()
        {
            return await _context.RoomTypes.ToListAsync();
        }
        public async Task<RoomType?> FindByIdAsync(int id)
        {
            return await _context.RoomTypes.FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task<RoomType> CreateAsync(RoomType request)
        {
            var result = await _context.RoomTypes.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task DeleteAsync(RoomType request)
        {
            _context.RoomTypes.Remove(request);
            await _context.SaveChangesAsync();
        }

        public async Task<RoomType> UpdateAsync(RoomType request)
        {
            _context.RoomTypes.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
