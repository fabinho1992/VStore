using AutoMapper;
using UserApi.Application.Dtos;
using UserApi.Domain;

namespace UserApi.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User,UserResponse>().ReverseMap();
        }
    }
}
