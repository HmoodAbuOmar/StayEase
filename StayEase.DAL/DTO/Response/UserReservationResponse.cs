using StayEase.DAL.Models;
using System.Text.Json.Serialization;

namespace StayEase.DAL.DTO.Response
{
    public class UserReservationResponse
    {
        public int Id { get; set; }
        public string? RoomNumber { get; set; }
        public decimal RoomPricePerNight { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReservationStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
