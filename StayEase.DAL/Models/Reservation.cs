namespace StayEase.DAL.Models
{
    public enum ReservationStatus
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3
    }
    public class Reservation : BaseModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public string? Notes { get; set; }
    }
}
