namespace StayEase.DAL.DTO.Response
{
    public class HotelReviewsResponse
    {
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<ReviewResponse> Reviews { get; set; } = new();
    }
}
