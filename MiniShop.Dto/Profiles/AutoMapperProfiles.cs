using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MiniShop.Model;
using System.Collections.Generic;

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

            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierCreateDto, Supplier>();
            CreateMap<SupplierUpdateDto, Supplier>();
            CreateMap<Supplier, SupplierCreateDto>();
            CreateMap<Supplier, SupplierUpdateDto>();
            CreateMap<SupplierDto, SupplierCreateDto>();
            CreateMap<SupplierDto, SupplierUpdateDto>();

            CreateMap<Item, ItemDto>()
                .ForMember(d => d.CategorieName, opt => opt.MapFrom(i => i.Categorie.Name))
                .ForMember(d => d.UnitName, opt => opt.MapFrom(i => i.Unit.Name))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(i => i.Supplier.Name));
            CreateMap<ItemCreateDto, Item>();
            CreateMap<ItemUpdateDto, Item>();
            CreateMap<Item, ItemCreateDto>()
                .ForMember(d => d.CategorieName, opt => opt.MapFrom(i => i.Categorie.Name))
                .ForMember(d => d.UnitName, opt => opt.MapFrom(i => i.Unit.Name))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(i => i.Supplier.Name));
            CreateMap<Item, ItemUpdateDto>()
                .ForMember(d => d.CategorieName, opt => opt.MapFrom(i => i.Categorie.Name))
                .ForMember(d => d.UnitName, opt => opt.MapFrom(i => i.Unit.Name))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(i => i.Supplier.Name));
            CreateMap<ItemDto, ItemCreateDto>();
            CreateMap<ItemDto, ItemUpdateDto>();


            // 目前还找不到集合里的映射处理！！！暂时在获取数据时做处理
            CreateMap<PurchaseOder, PurchaseOderDto>();
            CreateMap<PurchaseOderCreateDto, PurchaseOder>();
            CreateMap<PurchaseOderUpdateDto, PurchaseOder>();
            CreateMap<PurchaseOder, PurchaseOderCreateDto>();
            CreateMap<PurchaseOder, PurchaseOderUpdateDto>();
            CreateMap<PurchaseOderDto, PurchaseOderCreateDto>();
            CreateMap<PurchaseOderDto, PurchaseOderUpdateDto>();

            CreateMap<PurchaseOderItem, PurchaseOderItemDto>()
                .ForMember(d => d.ItemId, opt => opt.MapFrom(i => i.Item.Id))
                .ForMember(d => d.ItemCode, opt => opt.MapFrom(i => i.Item.Code))
                .ForMember(d => d.ItemName, opt => opt.MapFrom(i => i.Item.Name))
                .ForMember(d => d.UnitName, opt => opt.MapFrom(i => i.Item.Unit.Name))
                .ForMember(d => d.PurchasePrice, opt => opt.MapFrom(i => i.Item.PurchasePrice));
            CreateMap<PurchaseOderItemCreateDto, PurchaseOderItem>();
            CreateMap<PurchaseOderItemUpdateDto, PurchaseOderItem>();
            CreateMap<PurchaseOderItem, PurchaseOderItemCreateDto>();
            CreateMap<PurchaseOderItem, PurchaseOderItemUpdateDto>();
            CreateMap<PurchaseOderItemDto, PurchaseOderItemCreateDto>();
            CreateMap<PurchaseOderItemDto, PurchaseOderItemUpdateDto>();

        }
    }
}
