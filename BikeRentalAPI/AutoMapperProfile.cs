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
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>();

            // Bike mappings
            CreateMap<Bike, BikeDTO>();
            CreateMap<CreateBikeDTO, Bike>();
            CreateMap<UpdateBikeDTO, Bike>();

            // Rental mappings  
            CreateMap<Rental, RentalDTO>();
            CreateMap<CreateRentalDTO, Rental>();
        }
    }
}