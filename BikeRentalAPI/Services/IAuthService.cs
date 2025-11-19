using BikeRentalAPI.Models;
using BikeRentalAPI.Models.DTO;

namespace BikeRentalAPI.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterRequestDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);
        string GenerateJwtToken(User user);
    }
}