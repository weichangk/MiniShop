﻿using AutoMapper;
using MiniShop.Model;

namespace MiniShop.Dto.Profiles
{
    public class ShopProfile : Profile
    {
        public ShopProfile()
        {
            CreateMap<Shop, ShopInfoDto>();
            CreateMap<ShopCreateDto, Shop>();
        }
    }
}
