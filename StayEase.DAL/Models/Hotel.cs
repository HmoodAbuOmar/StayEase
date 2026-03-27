namespace StayEase.DAL.Models
{
    public class Hotel : BaseModel
    {
        public int Id { get; set; }
        public int StarRating { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<HotelTranslation> Translations { get; set; }
        public ICollection<Room> Rooms { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
