namespace StayEase.DAL.DTO.Response
{
    public class CheckoutResponse
    {
        public string? Url { get; set; }
        public string? PaymentId { get; set; }

        public bool Success { get; set; }

        public string? Message { get; set; }
    }
}
