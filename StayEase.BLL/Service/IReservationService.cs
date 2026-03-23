using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;

namespace StayEase.BLL.Service
{
    public interface IReservationService
    {
        public Task<List<UserReservationResponse>> GetMyReservationsAsync(string userId);
        public Task<BaseResponse> CreateReservationAsync(string userId, ReservationRequest request);
        public Task<BaseResponse> UpdateReservationAsync(int id, string userId, ReservationRequest request);
        public Task<BaseResponse> DeleteReservationAsync(string userId, int id);


        public Task<List<AdminReservationResponse>> GetAllReservationsForAdminAsync();
        public Task<AdminReservationResponse> GetAllReservationsByIdForAdminAsync(int id);

        public Task<BaseResponse> ConfirmReservationForAdminAsync(int id);
        public Task<BaseResponse> CancelReservationForAdminAsync(int id);


    }
}
