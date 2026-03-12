namespace StayEase.DAL.DTO.Request
{
    public class ResetPasswordRequest
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
