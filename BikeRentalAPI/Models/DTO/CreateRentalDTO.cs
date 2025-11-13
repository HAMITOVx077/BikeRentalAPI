using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models.DTO
{
    public class CreateRentalDTO
    {
        [Required]
        public int BikeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RentalStatusId { get; set; }
    }
}
