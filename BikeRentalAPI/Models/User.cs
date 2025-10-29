using System.ComponentModel.DataAnnotations;

namespace BikeRentalAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        //внешний ключ и навигационное свойство для Role
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        //связь с арендами
        public List<Rental> Rentals { get; set; } = new();
    }
}
