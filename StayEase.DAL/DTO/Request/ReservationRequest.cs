using System.ComponentModel.DataAnnotations;

namespace StayEase.DAL.DTO.Request
{
    public class ReservationRequest
    {
        [Required]
        public string RoomNumber { get; set; } = null!;
        [Required]
        public DateOnly CheckInDate { get; set; }

        [Required]
        public DateOnly CheckOutDate { get; set; }

        [Range(1, 4)]
        public int NumberOfGuests { get; set; }
        public string? Notes { get; set; }
    }
}
