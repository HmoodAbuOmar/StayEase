using System.ComponentModel.DataAnnotations;

namespace StayEase.DAL.DTO.Request
{
    public class UpdateRoomRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Invalid room type ID.")]
        public int RoomTypeId { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        public string RoomNumber { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; } = true;

    }
}
