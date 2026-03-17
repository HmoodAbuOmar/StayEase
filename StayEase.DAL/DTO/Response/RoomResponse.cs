namespace StayEase.DAL.DTO.Response
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; } = null!;
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = null!;
    }
}
