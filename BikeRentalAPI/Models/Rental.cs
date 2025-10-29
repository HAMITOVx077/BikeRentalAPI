using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Range(0, 10000)]
        public decimal TotalCost { get; set; }

        //внешние ключи
        public int BikeId { get; set; }
        public int UserId { get; set; }
        public int RentalStatusId { get; set; }

        //навигационные свойства
        public Bike Bike { get; set; } = null!;
        public User User { get; set; } = null!;
        public RentalStatus RentalStatus { get; set; } = null!;
    }
}
