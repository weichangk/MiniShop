using MiniShop.Dto;
using MiniShop.Mvc.Code;
using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [ApiRequestTokenFilter]
    [JsonReturn]
    public interface IShopApi : IHttpApi
    {
        [HttpGet("/api/Shop")]
        ITask<List<ShopInfoDto>> GetShops();

    }
}
