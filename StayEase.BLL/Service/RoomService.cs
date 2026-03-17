using Mapster;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using StayEase.DAL.Repository;

namespace StayEase.BLL.Service
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository,
            IRoomTypeRepository roomTypeRepository)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _roomTypeRepository = roomTypeRepository;
        }
        public async Task<List<RoomResponse>> GetAllRoomsAsync(string lang = "en")
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            var response = rooms.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<RoomResponse>>();
            return response;
        }

        public async Task<RoomResponse?> FindRoomByIdAsync(int id, string lang = "en")
        {
            var room = await _roomRepository.FindRoomByIdAsync(id);
            if (room is null)
            {
                return null;
            }
            var response = room.BuildAdapter()
               .AddParameters("lang", lang)
               .AdaptToType<RoomResponse>();
            return response;
        }
        public async Task<BaseResponse> CreateRoomAsync(CreateRoomRequest request)
        {
            var hotel = await _hotelRepository.FindByIdAsync(request.HotelId);
            if (hotel is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Hotel not found",
                    Errors = new List<string> { "Hotel with the specified ID does not exist." }
                };
            }
            var roomType = await _roomTypeRepository.FindByIdAsync(request.RoomTypeId);
            if (roomType is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Room type not found",
                    Errors = new List<string> { "Room type with the specified ID does not exist." }
                };
            }

            var room = request.Adapt<Room>();
            await _roomRepository.CreateRoomAsync(room);
            return new BaseResponse
            {
                Success = true,
                Message = "Room created successfully"
            };
        }

        public async Task<BaseResponse> DeleteRoomAsync(int Id)
        {
            var response = await _roomRepository.FindRoomByIdAsync(Id);
            if (response is null)
            {
                return new BaseResponse
                {
                    Message = "Room not found",
                    Success = false,
                    Errors = new List<string> { "Room with the specified ID does not exist." }
                };
            }

            await _roomRepository.DeleteRoomAsync(response);

            return new BaseResponse
            {
                Success = true,
                Message = "Room deleted successfully"
            };
        }



        public async Task<BaseResponse> UpdateRoomAsync(int Id, UpdateRoomRequest request)
        {
            var response = await _roomRepository.FindRoomByIdAsync(Id);
            if (response is null)
            {
                return new BaseResponse
                {
                    Message = "Room not found",
                    Success = false,
                    Errors = new List<string> { "Room with the specified ID does not exist." }
                };
            }
            request.Adapt(response); // Rooms.RoomNumber = request.RoomNumber
            await _roomRepository.UpdateRoomAsync(response);

            return new BaseResponse
            {
                Success = true,
                Message = "Room updated successfully"

            };
        }

        public async Task<List<RoomResponse>> GetAllAvaliableRoomAsync(string lang = "en")
        {
            var rooms = await _roomRepository.GetAvailableRoomsAsync();
            if (rooms is null)
            {
                return null;
            }
            var response = rooms.BuildAdapter()
              .AddParameters("lang", lang)
              .AdaptToType<List<RoomResponse>>();
            return response;
        }
    }
}
