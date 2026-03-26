using StayEase.DAL.Models;
using System.Text.Json.Serialization;

namespace StayEase.DAL.DTO.Request
{
    public class CheckoutRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
