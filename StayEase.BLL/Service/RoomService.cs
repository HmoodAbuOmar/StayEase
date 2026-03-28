using Mapster;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public RoomService(
            IRoomRepository roomRepository,
            IHotelRepository hotelRepository,
            IRoomTypeRepository roomTypeRepository,
            IMemoryCache cache)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _roomTypeRepository = roomTypeRepository;
            _cache = cache;
        }

        private static string GetAllRoomsCacheKey(string lang) => $"rooms:all:{lang}";
        private static string GetRoomByIdCacheKey(int id, string lang) => $"rooms:{id}:{lang}";
        private static string GetAvailableRoomsCacheKey(string lang) => $"rooms:available:{lang}";
        private static string GetRoomByNumberCacheKey(string roomNumber, string lang) => $"rooms:number:{roomNumber}:{lang}";

        public async Task<List<RoomResponse>> GetAllRoomsAsync(string lang = "en")
        {
            var cacheKey = GetAllRoomsCacheKey(lang);

            if (_cache.TryGetValue(cacheKey, out List<RoomResponse>? cachedRooms))
            {
                return cachedRooms!;
            }

            var rooms = await _roomRepository.GetAllRoomsAsync();

            var response = rooms.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<RoomResponse>>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, response, options);

            return response;
        }

        public async Task<RoomResponse?> FindRoomByIdAsync(int id, string lang = "en")
        {
            var cacheKey = GetRoomByIdCacheKey(id, lang);

            if (_cache.TryGetValue(cacheKey, out RoomResponse? cachedRoom))
            {
                return cachedRoom;
            }

            var room = await _roomRepository.FindRoomByIdAsync(id);
            if (room is null)
            {
                return null;
            }

            var response = room.BuildAdapter()
               .AddParameters("lang", lang)
               .AdaptToType<RoomResponse>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, response, options);

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

            InvalidateRoomCache(room.Id, room.RoomNumber);

            return new BaseResponse
            {
                Success = true,
                Message = "Room created successfully"
            };
        }

        public async Task<BaseResponse> DeleteRoomAsync(int id)
        {
            var response = await _roomRepository.FindRoomByIdAsync(id);
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

            InvalidateRoomCache(response.Id, response.RoomNumber);

            return new BaseResponse
            {
                Success = true,
                Message = "Room deleted successfully"
            };
        }

        public async Task<BaseResponse> UpdateRoomAsync(int id, UpdateRoomRequest request)
        {
            var response = await _roomRepository.FindRoomByIdAsync(id);
            if (response is null)
            {
                return new BaseResponse
                {
                    Message = "Room not found",
                    Success = false,
                    Errors = new List<string> { "Room with the specified ID does not exist." }
                };
            }

            var oldRoomNumber = response.RoomNumber;

            request.Adapt(response);
            await _roomRepository.UpdateRoomAsync(response);

            InvalidateRoomCache(response.Id, oldRoomNumber, response.RoomNumber);

            return new BaseResponse
            {
                Success = true,
                Message = "Room updated successfully"
            };
        }

        public async Task<List<RoomResponse>> GetAllAvaliableRoomAsync(string lang = "en")
        {
            var cacheKey = GetAvailableRoomsCacheKey(lang);

            if (_cache.TryGetValue(cacheKey, out List<RoomResponse>? cachedRooms))
            {
                return cachedRooms!;
            }

            var rooms = await _roomRepository.GetAvailableRoomsAsync();
            if (rooms is null)
            {
                return new List<RoomResponse>();
            }

            var response = rooms.BuildAdapter()
              .AddParameters("lang", lang)
              .AdaptToType<List<RoomResponse>>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, response, options);

            return response;
        }

        public async Task<RoomResponse?> GetByRoomNumberAsync(string id, string lang = "en")
        {
            var cacheKey = GetRoomByNumberCacheKey(id, lang);

            if (_cache.TryGetValue(cacheKey, out RoomResponse? cachedRoom))
            {
                return cachedRoom;
            }

            var room = await _roomRepository.GetByRoomNumberAsync(id);

            if (room is null)
            {
                return null;
            }

            var response = room.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<RoomResponse>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, response, options);

            return response;
        }

        private void InvalidateRoomCache(int roomId, string? oldRoomNumber = null, string? newRoomNumber = null)
        {
            foreach (var lang in new[] { "en", "ar" })
            {
                _cache.Remove(GetAllRoomsCacheKey(lang));
                _cache.Remove(GetAvailableRoomsCacheKey(lang));
                _cache.Remove(GetRoomByIdCacheKey(roomId, lang));

                if (!string.IsNullOrWhiteSpace(oldRoomNumber))
                    _cache.Remove(GetRoomByNumberCacheKey(oldRoomNumber, lang));

                if (!string.IsNullOrWhiteSpace(newRoomNumber))
                    _cache.Remove(GetRoomByNumberCacheKey(newRoomNumber, lang));
            }
        }
    }
}