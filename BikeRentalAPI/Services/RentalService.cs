using BikeRentalAPI.Models;
using BikeRentalAPI.Repositories.Interfaces;

namespace BikeRentalAPI.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBikeRepository _bikeRepository;
        private readonly IUserRepository _userRepository;

        //константы для статусов аренды
        private const int ACTIVE_STATUS_ID = 1;
        private const int COMPLETED_STATUS_ID = 2;
        private const int CANCELLED_STATUS_ID = 3;

        public RentalService(
            IRentalRepository rentalRepository,
            IBikeRepository bikeRepository,
            IUserRepository userRepository)
        {
            _rentalRepository = rentalRepository;
            _bikeRepository = bikeRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
        {
            return await _rentalRepository.GetAllAsync();
        }

        public async Task<Rental?> GetRentalByIdAsync(int id)
        {
            return await _rentalRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _rentalRepository.GetActiveRentalsAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId)
        {
            return await _rentalRepository.GetByUserIdAsync(userId);
        }

        public async Task<Rental> CreateRentalAsync(Rental rental)
        {
            //проверяем существование пользователя
            var user = await _userRepository.GetByIdAsync(rental.UserId);
            if (user == null)
                throw new Exception("Пользователь не найден");

            //проверяем существование велосипеда и его доступность
            var bike = await _bikeRepository.GetByIdAsync(rental.BikeId);
            if (bike == null)
                throw new Exception("Велосипед не найден");

            if (!bike.IsAvailable)
                throw new Exception("Велосипед уже арендован");

            //создаем аренду
            rental.StartTime = DateTime.UtcNow;
            rental.TotalCost = 0;
            rental.RentalStatusId = ACTIVE_STATUS_ID; //устанавливаем статус "Активная"

            var createdRental = await _rentalRepository.CreateAsync(rental);

            //помечаем велосипед как занятый
            bike.IsAvailable = false;
            await _bikeRepository.UpdateAsync(bike);

            return createdRental;
        }

        public async Task<decimal> CompleteRentalAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
                throw new Exception("Аренда не найдена");

            if (rental.EndTime.HasValue)
                throw new Exception("Аренда уже завершена");

            //устанавливаем время окончания
            rental.EndTime = DateTime.UtcNow;

            //рассчитываем стоимость
            var duration = rental.EndTime.Value - rental.StartTime;
            var bike = await _bikeRepository.GetByIdAsync(rental.BikeId);

            //расчет: цена в час * количество часов
            var hours = (decimal)Math.Ceiling(duration.TotalHours);
            rental.TotalCost = hours * bike.PricePerHour;

            //обновляем статус аренды на "Завершена"
            rental.RentalStatusId = COMPLETED_STATUS_ID;

            //освобождаем велосипед
            bike.IsAvailable = true;
            await _bikeRepository.UpdateAsync(bike);

            await _rentalRepository.UpdateAsync(rental);
            return rental.TotalCost;
        }

        public async Task<bool> DeleteRentalAsync(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            if (rental == null)
                return false;

            //если аренда активна, освобождаем велосипед
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