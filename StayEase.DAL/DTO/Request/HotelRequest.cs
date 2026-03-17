using System.ComponentModel.DataAnnotations;

namespace StayEase.DAL.DTO.Request
{
    public class HotelRequest
    {

        [Range(1, 5, ErrorMessage = "Star rating must be between 1 and 5.")]
        public int StarRating { get; set; }
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Translations are required.")]
        [MinLength(1, ErrorMessage = "At least one translation is required.")]
        public ICollection<HotelTranslationsRequest> Translations { get; set; }
    }
}
