namespace StayEase.DAL.Models
{
    public class Room : BaseModel
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Hotel Hotel { get; set; } = null!;
        public int HotelId { get; set; }
        public RoomType RoomType { get; set; } = null!;
        public int RoomTypeId { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

    }
}
