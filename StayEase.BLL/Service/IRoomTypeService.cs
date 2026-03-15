using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;

namespace StayEase.BLL.Service
{
    public interface IRoomTypeService
    {
        public Task<List<RoomTypeResponse>> GetAllAsync();
        public Task<RoomTypeResponse> GetByIdAsync(int id);
        public Task<RoomTypeResponse> CreateAsync(RoomTypeRequest request);
        public Task<BaseResponse> UpdateAsync(int id, RoomTypeRequest request);
        public Task<BaseResponse> DeleteAsync(int id);

    }
}
