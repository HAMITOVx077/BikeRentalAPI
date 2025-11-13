using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models.DTO
{
    public class UpdateUserDTO
    {
        [Required]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
