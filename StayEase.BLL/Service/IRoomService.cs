using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;

namespace StayEase.BLL.Service
{
    public interface IRoomService
    {
        public Task<List<RoomResponse>> GetAllRoomsAsync(string lang = "en");
        public Task<RoomResponse?> FindRoomByIdAsync(int id, string lang = "en");
        public Task<RoomResponse?> GetByRoomNumberAsync(string id, string lang = "en");
        public Task<BaseResponse> CreateRoomAsync(CreateRoomRequest request);
        public Task<BaseResponse> UpdateRoomAsync(int Id, UpdateRoomRequest request);
        public Task<BaseResponse> DeleteRoomAsync(int Id);
        public Task<List<RoomResponse>> GetAllAvaliableRoomAsync(string lang = "en");
    }
}
