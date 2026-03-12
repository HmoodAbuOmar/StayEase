namespace StayEase.DAL.Models
{
    public class BaseModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
