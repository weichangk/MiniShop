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
            CreateMap<CategorieCreateDto, Categorie>();
            CreateMap<CategorieUpdateDto, Categorie>();
            CreateMap<Categorie, CategorieCreateDto>();
            CreateMap<Categorie, CategorieUpdateDto>();
            CreateMap<CategorieDto, CategorieCreateDto>();
            CreateMap<CategorieDto, CategorieUpdateDto>();

            CreateMap<Unit, UnitDto>();
            CreateMap<UnitCreateDto, Unit>();
            CreateMap<UnitUpdateDto, Unit>();
            CreateMap<Unit, UnitCreateDto>();
            CreateMap<Unit, UnitUpdateDto>();
            CreateMap<UnitDto, UnitCreateDto>();
            CreateMap<UnitDto, UnitUpdateDto>();

            CreateMap<Item, ItemDto>();

        }
    }
}
