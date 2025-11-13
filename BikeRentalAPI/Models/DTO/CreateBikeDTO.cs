using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models.DTO
{
    public class CreateBikeDTO
    {
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        [Range(0.01, 1000)]
        public decimal PricePerHour { get; set; }
    }
}
