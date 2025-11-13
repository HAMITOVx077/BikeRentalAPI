using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models.DTO
{
    public class CreateUserDTO
    {
        [Required]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public int RoleId { get; set; }
    }
}
