using Microsoft.AspNetCore.Mvc;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IBikeRepository _bikeRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRentalStatusRepository _rentalStatusRepository;

        public TestController(
            IUserRepository userRepository,
            IBikeRepository bikeRepository,
            IRentalRepository rentalRepository,
            IRoleRepository roleRepository,
            IRentalStatusRepository rentalStatusRepository)
        {
            _userRepository = userRepository;
            _bikeRepository = bikeRepository;
            _rentalRepository = rentalRepository;
            _roleRepository = roleRepository;
            _rentalStatusRepository = rentalStatusRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("bikes")]
        public async Task<IActionResult> GetBikes()
        {
            var bikes = await _bikeRepository.GetAllAsync();
            return Ok(bikes);
        }

        [HttpGet("bikes/available")]
        public async Task<IActionResult> GetAvailableBikes()
        {
            var bikes = await _bikeRepository.GetAvailableBikesAsync();
            return Ok(bikes);
        }

        [HttpGet("rentals")]
        public async Task<IActionResult> GetRentals()
        {
            var rentals = await _rentalRepository.GetAllAsync();
            return Ok(rentals);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleRepository.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("rental-statuses")]
        public async Task<IActionResult> GetRentalStatuses()
        {
            var statuses = await _rentalStatusRepository.GetAllAsync();
            return Ok(statuses);
        }
    }
}