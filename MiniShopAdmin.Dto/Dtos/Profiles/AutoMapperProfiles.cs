using AutoMapper;
using MiniShopAdmin.Model;

namespace MiniShopAdmin.Dto.Profiles
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
