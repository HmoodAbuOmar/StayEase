using Mapster;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using StayEase.DAL.Repository;

namespace StayEase.BLL.Service
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<List<HotelResponse>> GetAllAsync(string lang = "en")
        {
            var hotels = await _hotelRepository.GetAllAsync();

            var response = hotels.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<HotelResponse>>();

            return response;
        }

        public async Task<HotelResponse?> GetByIdAsync(int id, string lang = "en")
        {
            var hotel = await _hotelRepository.FindByIdAsync(id);
            if (hotel is null)
            {
                return null;
            }
            var response = hotel.BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<HotelResponse>();

            return response;
        }

        public async Task<HotelResponse> Create(HotelRequest request)
        {
            var hotel = request.Adapt<Hotel>();
            await _hotelRepository.CreateAsync(hotel);
            return hotel.Adapt<HotelResponse>();
        }

        public async Task<BaseResponse> UpdateHotelAsync(int id, HotelRequest Request)
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
            if (Request.Translations != null)
            {
                foreach (var translation in Request.Translations)
                {
                    var existing = hotels.Translations.FirstOrDefault(t => t.Language == translation.Language);

                    if (existing is not null)
                    {
                        existing.Language = translation.Language;
                        existing.Name = translation.Name;
                        existing.City = translation.City;
                        existing.Country = translation.Country;
                        hotels.StarRating = Request.StarRating;
                        hotels.IsActive = Request.IsActive;
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
            await _hotelRepository.UpdateAsync(hotels);
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
            return new BaseResponse
            {
                Success = true,
                Message = "Hotel deleted successfully",
            };
        }

        public async Task<List<HotelResponse>> GetActiveHotelAsync()
        {
            var hotel = await _hotelRepository.GetActiveHotelAsync();
            if (hotel is null)
            {
                return null;
            }
            var response = hotel.Adapt<List<HotelResponse>>();
            return response;
        }
    }
}
