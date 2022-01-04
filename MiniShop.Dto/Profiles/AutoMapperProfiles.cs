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
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserCreateDto>();
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserUpdateDto, User>();

            CreateMap<Store, StoreDto>();
            CreateMap<StoreCreateDto, Store>();
            CreateMap<StoreUpdateDto, Store>();

            CreateMap<Categorie, CategorieDto>();
            CreateMap<CategorieDto, Categorie>();

            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();

        }
    }
}
