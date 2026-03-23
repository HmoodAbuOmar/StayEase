using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Data;
using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateReservationAsync(Reservation request)
        {
            await _context.Reservations.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(Reservation request)
        {
            _context.Reservations.Remove(request);
            await _context.SaveChangesAsync();
        }


        public async Task<Reservation?> GetMyReservationByIdAsync(string userId, int id)
        {
            return await _context.Reservations
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id);
        }

        public async Task<List<Reservation>> GetMyReservationsAsync(string userId)
        {
            return await _context.Reservations
           .Include(r => r.Room)
           .Where(r => r.UserId == userId)
           .ToListAsync();
        }

        public async Task<bool> HasConflictAsync(int roomId, DateOnly checkInDate, DateOnly checkOutDate, int? reservationId = null)
        {
            return await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.Status != ReservationStatus.Cancelled &&
                (!reservationId.HasValue || r.Id != reservationId.Value) &&
                checkInDate < r.CheckOutDate &&
                checkOutDate > r.CheckInDate
            );
        }

        public async Task UpdateReservationAsync(Reservation request)
        {
            _context.Reservations.Update(request);
            await _context.SaveChangesAsync();
        }

        //******************** for Admin
        public async Task<List<Reservation>> GetAllReservationsForAdminAsync()
        {
            return await _context.Reservations
              .Include(r => r.Room)
               .Include(r => r.ApplicationUser)
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdForAdminAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ConfirmReservationForAdminAsync(int id)
        {
            var result = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            result.Status = ReservationStatus.Confirmed;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelledReservationForAdminAsync(int id)
        {
            var result = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            result.Status = ReservationStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
