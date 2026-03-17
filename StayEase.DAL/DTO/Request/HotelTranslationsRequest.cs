using System.ComponentModel.DataAnnotations;

namespace StayEase.DAL.DTO.Request
{
    public class HotelTranslationsRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; } = null!;
        public string? Language { get; set; }
    }
}
