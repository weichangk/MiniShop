using MiniShop.Dto;
using MiniShop.Mvc.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [TokenFilter]
    [JsonReturn]
    public interface IShopApi : IHttpApi
    {
        [HttpGet("/api/Shop")]
        ITask<List<ShopInfoDto>> GetShops();

    }
}
