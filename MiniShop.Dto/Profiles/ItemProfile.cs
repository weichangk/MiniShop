using AutoMapper;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemCreateDto, Item>();
        }
    }
}
