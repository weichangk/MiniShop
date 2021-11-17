using AutoMapper;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Shop, ShopInfoDto>();
            CreateMap<ShopCreateDto, Shop>();
        }
    }
}
