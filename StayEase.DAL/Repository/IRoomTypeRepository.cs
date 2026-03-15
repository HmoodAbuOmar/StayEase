using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public interface IRoomTypeRepository
    {
        Task<List<RoomType>> GetAllAsync();
        Task<RoomType?> FindByIdAsync(int id);
        Task<RoomType> CreateAsync(RoomType request);
        Task<RoomType> UpdateAsync(RoomType request);
        Task DeleteAsync(RoomType request);



    }
}
