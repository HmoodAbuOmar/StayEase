using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Data;
using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .ThenInclude(r => r.Translations)
                .Include(r => r.RoomType).ToListAsync();
        }
        public async Task CreateRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }
        public async Task<Room?> FindRoomByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .ThenInclude(r => r.Translations)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateRoomAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomAsync(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

        public Task<List<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Room>> GetAvailableRoomsAsync()
        {
            return _context.Rooms
                .Include(r => r.Hotel)
                .ThenInclude(r => r.Translations)
                .Include(r => r.RoomType)
                .Where(r => r.IsAvailable)
                .ToListAsync();
        }
    }
}
