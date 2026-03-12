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

        public List<HotelResponse> GetAll()
        {
            var hotels = _hotelRepository.GetAll();
            var response = hotels.Adapt<List<HotelResponse>>();
            return response;
        }
        public HotelResponse Create(HotelRequest request)
        {
            var hotel = request.Adapt<Hotel>();
            _hotelRepository.Create(hotel);
            return hotel.Adapt<HotelResponse>();
        }

    }
}
