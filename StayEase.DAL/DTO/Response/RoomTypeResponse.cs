namespace StayEase.DAL.DTO.Response
{
    public class RoomTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int MaxCapacity { get; set; }
        public bool IsActive { get; set; }

    }
}
