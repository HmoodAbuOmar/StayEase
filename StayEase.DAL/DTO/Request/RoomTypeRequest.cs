namespace StayEase.DAL.DTO.Request
{
    public class RoomTypeRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public bool IsActive { get; set; }

    }
}
