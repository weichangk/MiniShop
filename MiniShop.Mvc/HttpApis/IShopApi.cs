using MiniShop.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [JsonReturn]
    public interface IShopApi : IHttpApi
    {
        [HttpGet("/api/shop")]
        ITask<List<ShopInfoDto>> GetShops();

    }
}
