using MiniShop.Mvc.Code;
using MiniShopAdmin.Dto;
using Orm.Core.Result;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [MiniShopAdminApi]
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IRenewPackageApi : IHttpApi
    {
        [HttpGet("/api/Admin/RenewPackage")]
        ITask<ResultModel<List<RenewPackageDto>>> GetRenewPackagesAsync();

        [HttpGet("/api/Admin/RenewPackage/GetById")]
        ITask<ResultModel<RenewPackageDto>> GetRenewPackageByIdAsync(int id);
    }
}
