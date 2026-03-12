namespace StayEase.DAL.Models
{
    public class Hotel : BaseModel
    {
        public int Id { get; set; }
        public int StarRating { get; set; }
        public bool IsActive { get; set; } = true;
        public List<HotelTranslation> Translations { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
