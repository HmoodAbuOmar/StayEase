using System.ComponentModel.DataAnnotations;

namespace StayEase.DAL.DTO.Request
{
    public class CreateRoomRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid hotel ID.")]
        public int HotelId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid room type ID.")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        [StringLength(10, ErrorMessage = "Room number must not exceed 10 characters.")]
        public string RoomNumber { get; set; } = null!;

        [Range(typeof(decimal), "0.01", "9999999", ErrorMessage = "Price per night must be greater than zero.")]
        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
