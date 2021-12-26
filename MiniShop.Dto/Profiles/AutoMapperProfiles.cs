using AutoMapper;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Shop, ShopDto>();
            CreateMap<ShopDto, Shop>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserCreateDto, User>()
                .ForMember(d => d.Role, opt => opt.MapFrom(i => i.RoleName));
            CreateMap<User, UserCreateDto>()
                .ForMember(d => d.RoleName, opt => opt.MapFrom(i => i.Role));
            CreateMap<User, UserUpdateDto>()
                .ForMember(d => d.RoleName, opt => opt.MapFrom(i => i.Role));
            CreateMap<UserUpdateDto, User>()
                .ForMember(d => d.Role, opt => opt.MapFrom(i => i.RoleName));

            CreateMap<Categorie, CategorieDto>();
            CreateMap<CategorieDto, Categorie>();

            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();

        }
    }
}
