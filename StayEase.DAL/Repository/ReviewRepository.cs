using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Data;
using StayEase.DAL.Models;

namespace StayEase.DAL.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> HotelExistsAsync(int hotelId)
        {
            return await _context.Hotels.AnyAsync(h => h.Id == hotelId);
        }

        public async Task<bool> UserCanReviewHotelAsync(int hotelId, string userId)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Payments)
                .AnyAsync(r =>
                    r.UserId == userId &&
                    r.Room.HotelId == hotelId &&
                    r.Payments.Any(p => p.Status == PaymentStatus.Paid));
        }

        public async Task<bool> HasUserReviewedHotelAsync(int hotelId, string userId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.HotelId == hotelId && r.UserId == userId);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetByHotelAsync(int hotelId)
        {
            return await _context.Reviews
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

    }
}
