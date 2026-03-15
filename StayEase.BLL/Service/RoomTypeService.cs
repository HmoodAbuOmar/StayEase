using Mapster;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using StayEase.DAL.Repository;

namespace StayEase.BLL.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }
        public async Task<List<RoomTypeResponse>> GetAllAsync()
        {
            var roomTypes = await _roomTypeRepository.GetAllAsync();

            var result = roomTypes.Adapt<List<RoomTypeResponse>>();

            return result;
        }

        public async Task<RoomTypeResponse> GetByIdAsync(int id)
        {
            var roomType = await _roomTypeRepository.FindByIdAsync(id);

            if (roomType is null)
            {
                return null;
            }
            return roomType.Adapt<RoomTypeResponse>();
        }
        public async Task<RoomTypeResponse> CreateAsync(RoomTypeRequest request)
        {
            var roomType = request.Adapt<RoomType>();
            await _roomTypeRepository.CreateAsync(roomType);
            var result = roomType.Adapt<RoomTypeResponse>();
            return result;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var roomType = await _roomTypeRepository.FindByIdAsync(id);
            if (roomType is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Room type not found",
                    Errors = new List<string> { "Room type with the specified ID does not exist." }
                };
            }

            await _roomTypeRepository.DeleteAsync(roomType);
            return new BaseResponse
            {
                Success = true,
                Message = "Room type deleted successfully"
            };

        }
        public async Task<BaseResponse> UpdateAsync(int id, RoomTypeRequest request)
        {
            var roomType = await _roomTypeRepository.FindByIdAsync(id);
            if (roomType is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Room type not found",
                    Errors = new List<string> { "Room type with the specified ID does not exist." }
                };
            }
            request.Adapt(roomType);// ==>> roomType.Name = request.Name;
            await _roomTypeRepository.UpdateAsync(roomType);

            return new BaseResponse
            {
                Success = true,
                Message = "Room type updated successfully"
            };
        }
    }
}
