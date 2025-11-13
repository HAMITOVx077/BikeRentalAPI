namespace BikeRentalAPI.Models.DTO
{
    public class BikeDTO
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public decimal PricePerHour { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
