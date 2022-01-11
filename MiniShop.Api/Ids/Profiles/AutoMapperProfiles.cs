using AutoMapper;
using MiniShop.Api.Ids.Dto;

namespace MiniShop.Api.Ids.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdsUser, UserDto>();
            CreateMap<UserCreateDto, IdsUser>();
            CreateMap<UserUpdateDto, IdsUser>();
        }
    }
}
