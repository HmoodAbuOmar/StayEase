namespace StayEase.DAL.Models
{
    public class Review : BaseModel
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }

    }
}
