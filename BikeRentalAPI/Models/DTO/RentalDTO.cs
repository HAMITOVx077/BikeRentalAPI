namespace BikeRentalAPI.Models.DTO
{
    public class RentalDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal TotalCost { get; set; }
        public int BikeId { get; set; }
        public string BikeModel { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserLogin { get; set; } = string.Empty;
        public int RentalStatusId { get; set; }
        public string RentalStatusName { get; set; } = string.Empty;
    }
}
