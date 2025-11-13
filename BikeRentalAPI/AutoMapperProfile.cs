using AutoMapper;
using BikeRentalAPI.Models;
using BikeRentalAPI.Models.DTO;

namespace BikeRentalAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>(); //если нужно обратное преобразование

            // Bike mappings
            CreateMap<Bike, BikeDTO>();
            CreateMap<BikeDTO, Bike>();

            // Rental mappings  
            CreateMap<Rental, RentalDTO>();
            CreateMap<RentalDTO, Rental>();

            CreateMap<CreateBikeDTO, Bike>();
            CreateMap<CreateUserDTO, User>();
            CreateMap<CreateRentalDTO, Rental>();
        }
    }
}