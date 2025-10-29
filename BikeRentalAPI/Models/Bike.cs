using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models
{
    public class Bike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        [Range(0.01, 1000)]
        public decimal PricePerHour { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //связь с арендами
        public List<Rental> Rentals { get; set; } = new();
    }
}
