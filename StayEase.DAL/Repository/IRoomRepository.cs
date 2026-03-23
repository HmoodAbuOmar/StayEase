using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public interface IRoomRepository
    {
        public Task<List<Room>> GetAllRoomsAsync();
        public Task<Room?> FindRoomByIdAsync(int id);
        public Task<Room?> GetByRoomNumberAsync(string roomNumber);
        public Task CreateRoomAsync(Room room);
        public Task UpdateRoomAsync(Room room);
        public Task DeleteRoomAsync(Room room);
        public Task<List<Room>> GetRoomsByHotelIdAsync(int hotelId);

        //public Task<List<Room>> GetRoomsByRoomTypeIdAsync(int roomTypeId);
        public Task<List<Room>> GetAvailableRoomsAsync();

    }
}
