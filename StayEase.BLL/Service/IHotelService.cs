using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;

namespace StayEase.BLL.Service
{
    public interface IHotelService
    {
        Task<List<HotelResponse>> GetAllAsync(string lang = "en");
        Task<HotelResponse?> GetByIdAsync(int id, string lang = "en");
        Task<HotelResponse> Create(HotelRequest request);
        Task<BaseResponse> UpdateHotelAsync(int id, HotelRequest request);
        Task<BaseResponse> DeleteAsync(int id);
        Task<List<HotelResponse>> GetActiveHotelAsync();

    }
}
