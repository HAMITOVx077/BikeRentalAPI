using AutoMapper;
using BikeRentalAPI.Models.DTO;
using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;
using BikeRentalAPI.Services;

namespace BikeRentalAPI.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBikeRepository _bikeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RentalService(
            IRentalRepository rentalRepository,
            IBikeRepository bikeRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _bikeRepository = bikeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все аренды
        /// </summary>
        public async Task<IEnumerable<RentalDTO>> GetAllRentalsAsync()
        {
            var rentals = await _rentalRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RentalDTO>>(rentals);
        }

        /// <summary>
        /// Получить аренду по ID
        /// </summary>
        public async Task<RentalDTO?> GetRentalByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            return _mapper.Map<RentalDTO?>(rental);
        }

        /// <summary>
        /// Получить активные аренды
        /// </summary>
        public async Task<IEnumerable<RentalDTO>> GetActiveRentalsAsync()
        {
            var rentals = await _rentalRepository.GetActiveRentalsAsync();
            return _mapper.Map<IEnumerable<RentalDTO>>(rentals);
        }

        /// <summary>
        /// Получить аренды пользователя
        /// </summary>
        public async Task<IEnumerable<RentalDTO>> GetRentalsByUserIdAsync(int userId)
        {
            var rentals = await _rentalRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RentalDTO>>(rentals);
        }

        /// <summary>
        /// Создать новую аренду
        /// </summary>
        public async Task<RentalDTO> CreateRentalAsync(CreateRentalDTO createRentalDto)
        {
            // Проверяем существование пользователя
            var user = await _userRepository.GetByIdAsync(createRentalDto.UserId);
            if (user == null)
                throw new Exception("Пользователь не найден");

            // Проверяем существование велосипеда и его доступность
            var bike = await _bikeRepository.GetByIdAsync(createRentalDto.BikeId);
            if (bike == null)
                throw new Exception("Велосипед не найден");

            if (!bike.IsAvailable)
                throw new Exception("Велосипед уже арендован");

            // Создаем аренду
            var rental = _mapper.Map<Rental>(createRentalDto);
            rental.StartTime = DateTime.UtcNow;
            rental.TotalCost = 0;

            var createdRental = await _rentalRepository.CreateAsync(rental);

            // Помечаем велосипед как занятый
            bike.IsAvailable = false;
            await _bikeRepository.UpdateAsync(bike);

            return _mapper.Map<RentalDTO>(createdRental);
        }

        /// <summary>
        /// Завершить аренду
        /// </summary>
        public async Task<decimal> CompleteRentalAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
                throw new Exception("Аренда не найдена");

            if (rental.EndTime.HasValue)
                throw new Exception("Аренда уже завершена");

            // Устанавливаем время окончания
            rental.EndTime = DateTime.UtcNow;

            // Рассчитываем стоимость
            var duration = rental.EndTime.Value - rental.StartTime;
            var bike = await _bikeRepository.GetByIdAsync(rental.BikeId);

            // Расчет: цена в час * количество часов (округляем вверх)
            var hours = (decimal)Math.Ceiling(duration.TotalHours);
            rental.TotalCost = hours * bike.PricePerHour;

            // Освобождаем велосипед
            bike.IsAvailable = true;
            await _bikeRepository.UpdateAsync(bike);

            await _rentalRepository.UpdateAsync(rental);
            return rental.TotalCost;
        }

        /// <summary>
        /// Удалить аренду
        /// </summary>
        public async Task<bool> DeleteRentalAsync(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            if (rental == null)
                return false;

            // Если аренда активна, освобождаем велосипед
            if (!rental.EndTime.HasValue)
            {
                var bike = await _bikeRepository.GetByIdAsync(rental.BikeId);
                if (bike != null)
                {
                    bike.IsAvailable = true;
                    await _bikeRepository.UpdateAsync(bike);
                }
            }

            return await _rentalRepository.DeleteAsync(id);
        }
    }
}