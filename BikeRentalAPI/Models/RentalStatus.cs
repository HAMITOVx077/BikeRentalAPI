using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models
{
    public class RentalStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!; //Активный, Выполнен, Отменен

        public List<Rental> Rentals { get; set; } = new();
    }
}
