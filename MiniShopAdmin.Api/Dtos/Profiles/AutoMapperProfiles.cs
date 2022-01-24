using AutoMapper;
using MiniShopAdmin.Api.Models;

namespace MiniShopAdmin.Api.Dtos.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RenewPackage, RenewPackageDto>();
            CreateMap<RenewPackageCreateDto, RenewPackage>();
            CreateMap<RenewPackageUpdateDto, RenewPackage>();
        }
    }
}
