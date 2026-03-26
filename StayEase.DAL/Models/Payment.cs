namespace StayEase.DAL.Models
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3,
        Refunded = 4,
        Expired = 5
    }
    public enum PaymentMethod
    {
        Visa = 1,
        Cash = 2,

    }
    public class Payment : BaseModel
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public PaymentMethod Method { get; set; }

        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
