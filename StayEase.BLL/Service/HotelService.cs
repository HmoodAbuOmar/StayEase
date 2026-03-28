using Mapster;
using Microsoft.Extensions.Caching.Memory;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using StayEase.DAL.Repository;

namespace StayEase.BLL.Service
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMemoryCache _cache;

        public HotelService(IHotelRepository hotelRepository, IMemoryCache cache)
        {
            _hotelRepository = hotelRepository;
            _cache = cache;
        }

        private static string GetAllHotelsCacheKey(string lang) => $"hotels:all:{lang}";
        private static string GetHotelByIdCacheKey(int id, string lang) => $"hotels:{id}:{lang}";
        private static string GetActiveHotelsCacheKey(string lang) => $"hotels:active:{lang}";

        public async Task<List<HotelResponse>> GetAllAsync(string lang = "en")
        {
            var cacheKey = GetAllHotelsCacheKey(lang);

            if (_cache.TryGetValue(cacheKey, out List<HotelResponse>? cachedHotels))
            {
                return cachedHotels!;
            }

            var hotels = await _hotelRepository.GetAllAsync();

            var response = hotels.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<HotelResponse>>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, response, options);

            return response;
        }

        public async Task<HotelResponse?> GetByIdAsync(int id, string lang = "en")
        {
            var cacheKey = GetHotelByIdCacheKey(id, lang);

            if (_cache.TryGetValue(cacheKey, out HotelResponse? cachedHotel))
            {
                return cachedHotel;
            }

            var hotel = await _hotelRepository.FindByIdAsync(id);
            if (hotel is null)
            {
                return null;
            }

            var response = hotel.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<HotelResponse>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, response, options);

            return response;
        }

        public async Task<BaseResponse> Create(HotelRequest request)
        {
            var hotel = request.Adapt<Hotel>();
            await _hotelRepository.CreateAsync(hotel);

            InvalidateHotelCache(hotel.Id);

            return new BaseResponse
            {
                Message = "Hotel Created Successfully",
                Success = true,
            };
        }

        public async Task<BaseResponse> UpdateHotelAsync(int id, HotelRequest request)
        {
            var hotels = await _hotelRepository.FindByIdAsync(id);
            if (hotels is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Hotel not found",
                };
            }

            if (request.Translations != null)
            {
                foreach (var translation in request.Translations)
                {
                    var existing = hotels.Translations.FirstOrDefault(t => t.Language == translation.Language);

                    if (existing is not null)
                    {
                        existing.Language = translation.Language;
                        existing.Name = translation.Name;
                        existing.City = translation.City;
                        existing.Country = translation.Country;
                    }
                    else
                    {
                        return new BaseResponse
                        {
                            Success = false,
                            Message = $"Translation for language {translation.Language} not found",
                        };
                    }
                }
            }

            hotels.StarRating = request.StarRating;
            hotels.IsActive = request.IsActive;

            await _hotelRepository.UpdateAsync(hotels);

            InvalidateHotelCache(id);

            return new BaseResponse
            {
                Success = true,
                Message = "Hotel updated successfully",
            };
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var hotel = await _hotelRepository.FindByIdAsync(id);
            if (hotel is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Hotel not found",
                };
            }

            await _hotelRepository.DeleteAsync(hotel);

            InvalidateHotelCache(id);

            return new BaseResponse
            {
                Success = true,
                Message = "Hotel deleted successfully",
            };
        }

        public async Task<List<HotelResponse>> GetActiveHotelAsync(string lang = "en")
        {
            var cacheKey = GetActiveHotelsCacheKey(lang);

            if (_cache.TryGetValue(cacheKey, out List<HotelResponse>? cachedActiveHotels))
            {
                return cachedActiveHotels!;
            }

            var hotel = await _hotelRepository.GetActiveHotelAsync();
            if (hotel is null)
            {
                return new List<HotelResponse>();
            }

            var response = hotel.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<HotelResponse>>();

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(cacheKey, response, options);

            return response;
        }

        private void InvalidateHotelCache(int hotelId)
        {
            foreach (var lang in new[] { "en", "ar" })
            {
                _cache.Remove(GetAllHotelsCacheKey(lang));
                _cache.Remove(GetHotelByIdCacheKey(hotelId, lang));
                _cache.Remove(GetActiveHotelsCacheKey(lang));
            }
        }
    }
}