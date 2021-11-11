using AutoMapper;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class UserPrifile : Profile
    {
        public UserPrifile()
        {
            CreateMap<UserCreateDto, User>();
        }
    }
}
