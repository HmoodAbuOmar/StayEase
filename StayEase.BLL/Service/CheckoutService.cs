using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Data;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using Stripe.Checkout;

namespace StayEase.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ApplicationDbContext _context;

        public CheckoutService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId)
        {
            var reservation = await _context.Reservations
         .Include(r => r.Room)
         .Where(r => r.UserId == userId)
         .Where(r => !_context.Payments
             .Any(p => p.ReservationId == r.Id && p.Status == PaymentStatus.Paid))
         .OrderByDescending(r => r.Id)
         .FirstOrDefaultAsync();

            if (reservation is null)
                return new CheckoutResponse()
                {
                    Success = false,
                    Message = "No Have Reservations"
                };
            var RoomNumber = reservation.Room.RoomNumber;


            if (request.PaymentMethod == PaymentMethod.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "Cash payment created successfully"
                };
            }

            var payment = new Payment
            {
                ReservationId = reservation.Id,
                Amount = reservation.TotalPrice,
                Method = request.PaymentMethod,
                Status = PaymentStatus.Pending,

            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = $"https://localhost:7142/api/User/Checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"https://localhost:7142/api/user/checkouts/cancel",
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                    {
                            { "UserId", userId },
                            { "ReservationId", reservation.Id.ToString() },
                               { "PaymentId", payment.Id.ToString() }
                    },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(reservation.TotalPrice * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Reservation {reservation.Id}," +
                                $" " +
                                $"RoomNumber {RoomNumber}"
                            }
                        }
                    }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            payment.StripeSessionId = session.Id;
            await _context.SaveChangesAsync();

            return new CheckoutResponse
            {
                Url = session.Url,
                PaymentId = payment.Id.ToString(),
                Success = true,
                Message = "Card payment session created"
            };
        }
        public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            if (session is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Stripe session not found"
                };
            }

            if (session.PaymentStatus != "paid")
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Payment is not completed yet"
                };
            }

            var userId = session.Metadata.TryGetValue("UserId", out var u) ? u : null;
            var reservationId = session.Metadata.TryGetValue("ReservationId", out var r) ? r : null;
            var paymentId = session.Metadata.TryGetValue("PaymentId", out var p) ? p : null;

            var payment = await _context.Payments
                .Include(p => p.Reservation)
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId);

            if (payment is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Payment record not found"
                };
            }

            if (payment.Status == PaymentStatus.Paid)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    PaymentId = payment.Id.ToString(),
                    Message = "Payment already confirmed"
                };
            }

            payment.Status = PaymentStatus.Paid;
            payment.Reservation.Status = ReservationStatus.Confirmed;

            await _context.SaveChangesAsync();

            return new CheckoutResponse
            {
                Success = true,
                PaymentId = payment.Id.ToString(),
                Message = "Payment completed successfully"
            };
        }
    }
}
