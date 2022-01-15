using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdentityUser, UserDto>();
            CreateMap<UserCreateDto, IdentityUser>();
            CreateMap<IdentityUser, UserCreateDto>();
            CreateMap<UserUpdateDto, IdentityUser>();
            CreateMap<IdentityUser, UserUpdateDto>();
            CreateMap<UserDto, UserUpdateDto>();

            CreateMap<Shop, ShopDto>();
            CreateMap<ShopCreateDto, Shop>();
            CreateMap<ShopUpdateDto, Shop>();
            CreateMap<Shop, ShopCreateDto>();
            CreateMap<Shop, ShopUpdateDto>();
            CreateMap<ShopDto, ShopCreateDto>();
            CreateMap<ShopDto, ShopUpdateDto>();

            CreateMap<Store, StoreDto>();
            CreateMap<StoreCreateDto, Store>();
            CreateMap<StoreUpdateDto, Store>();
            CreateMap<Store, StoreCreateDto>();
            CreateMap<Store, StoreUpdateDto>();
            CreateMap<StoreDto, StoreCreateDto>();
            CreateMap<StoreDto, StoreUpdateDto>();

            CreateMap<Categorie, CategorieDto>();

            CreateMap<Item, ItemDto>();

        }
    }
}
