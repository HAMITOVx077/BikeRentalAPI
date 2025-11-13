using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models.DTO
{
    public class UpdateBikeDTO
    {
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        [Range(0.01, 1000)]
        public decimal PricePerHour { get; set; }
    }
}
