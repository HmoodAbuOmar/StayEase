namespace StayEase.DAL.DTO.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
