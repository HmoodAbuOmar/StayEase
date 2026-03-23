using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public interface IReservationRepository
    {
        public Task<List<Reservation>> GetMyReservationsAsync(string userId);
        public Task<Reservation?> GetMyReservationByIdAsync(string userId, int id);
        public Task CreateReservationAsync(Reservation request);
        public Task UpdateReservationAsync(Reservation request);
        public Task DeleteReservationAsync(Reservation request);
        public Task<bool> HasConflictAsync(int roomId, DateOnly checkInDate, DateOnly checkOutDate, int? reservationId = null);
        Task<List<Reservation>> GetAllReservationsForAdminAsync();
        public Task<Reservation> GetReservationByIdForAdminAsync(int id);
        public Task<bool> ConfirmReservationForAdminAsync(int id);
        public Task<bool> CancelledReservationForAdminAsync(int id);

    }
}
