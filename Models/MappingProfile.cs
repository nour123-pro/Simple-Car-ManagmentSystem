using AutoMapper;
using CarManagmentSystem.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Car, CarDto>().ReverseMap(); // Map Car entity to CarDto and vice versa
    }
}
