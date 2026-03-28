using Mapster;
using Microsoft.Extensions.Caching.Memory;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using StayEase.DAL.Repository;

namespace StayEase.BLL.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMemoryCache _cache;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository, IMemoryCache cache)
        {
            _roomTypeRepository = roomTypeRepository;
            _cache = cache;
        }

        private const string AllRoomTypesCacheKey = "roomtypes:all";
        private static string GetRoomTypeByIdCacheKey(int id) => $"roomtypes:{id}";

        public async Task<List<RoomTypeResponse>> GetAllAsync()
        {
            if (_cache.TryGetValue(AllRoomTypesCacheKey, out List<RoomTypeResponse>? cachedRoomTypes))
            {
                return cachedRoomTypes!;
            }

            var roomTypes = await _roomTypeRepository.GetAllAsync();

            var result = roomTypes.Adapt<List<RoomTypeResponse>>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(AllRoomTypesCacheKey, result, options);

            return result;
        }

        public async Task<RoomTypeResponse> GetByIdAsync(int id)
        {
            var cacheKey = GetRoomTypeByIdCacheKey(id);

            if (_cache.TryGetValue(cacheKey, out RoomTypeResponse? cachedRoomType))
            {
                return cachedRoomType!;
            }

            var roomType = await _roomTypeRepository.FindByIdAsync(id);

            if (roomType is null)
            {
                return null;
            }

            var result = roomType.Adapt<RoomTypeResponse>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, result, options);

            return result;
        }

        public async Task<RoomTypeResponse> CreateAsync(RoomTypeRequest request)
        {
            var roomType = request.Adapt<RoomType>();
            await _roomTypeRepository.CreateAsync(roomType);

            InvalidateRoomTypeCache(roomType.Id);

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

            InvalidateRoomTypeCache(id);

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

            request.Adapt(roomType);
            await _roomTypeRepository.UpdateAsync(roomType);

            InvalidateRoomTypeCache(id);

            return new BaseResponse
            {
                Success = true,
                Message = "Room type updated successfully"
            };
        }

        private void InvalidateRoomTypeCache(int roomTypeId)
        {
            _cache.Remove(AllRoomTypesCacheKey);
            _cache.Remove(GetRoomTypeByIdCacheKey(roomTypeId));
        }
    }
}