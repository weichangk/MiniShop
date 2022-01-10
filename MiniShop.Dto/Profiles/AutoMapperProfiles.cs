using AutoMapper;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Shop, ShopDto>();
            CreateMap<ShopUpdateDto, Shop>();

            CreateMap<Store, StoreDto>();
            CreateMap<StoreCreateDto, Store>();
            CreateMap<StoreUpdateDto, Store>();

            CreateMap<Categorie, CategorieDto>();

            CreateMap<Item, ItemDto>();

        }
    }
}
