using System.ComponentModel.DataAnnotations;

namespace StayEase.DAL.DTO.Request
{
    public class RoomTypeRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [Range(1, 10, ErrorMessage = "Max capacity must be between 1 and 10.")]
        public int MaxCapacity { get; set; }

        public bool IsActive { get; set; }

    }
}
