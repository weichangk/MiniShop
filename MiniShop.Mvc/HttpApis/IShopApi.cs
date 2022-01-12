using MiniShop.Dto;
using MiniShop.Mvc.Code;
using System;
using WebApiClient;
using WebApiClient.Attributes;
using Orm.Core.Result;

namespace MiniShop.Mvc.HttpApis
{
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IShopApi : IHttpApi
    {
        /// <summary>
        /// 根据商店ID，返回商店信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("/api/Shop/{shopId}")]
        ITask<ResultModel<ShopDto>> QueryAsync(Guid shopId);

        /// <summary>
        /// 修改商店
        /// </summary>
        /// <returns></returns>
        [HttpPut("/api/Shop")]
        ITask<ResultModel<ShopDto>> UpdateAsync([JsonContent] ShopDto model);
    }
}
