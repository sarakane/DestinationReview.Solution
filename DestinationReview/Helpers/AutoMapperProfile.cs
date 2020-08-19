using AutoMapper;
using DestinationReview.Dtos;
using DestinationReview.Models;

namespace DestinationReview.Helpers
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<UserDto, UserDto>();
      CreateMap<UserDto, User>();
      CreateMap<User, UserDto>();
    }
  }
}