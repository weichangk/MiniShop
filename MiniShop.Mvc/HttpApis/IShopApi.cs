using MiniShop.Dto;
using MiniShop.Mvc.Code;
using System;
using System.Collections.Generic;
using System.Net.Http;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [ApiRequestTokenFilter]
    [JsonReturn]
    public interface IShopApi : IHttpApi
    {
        /// <summary>
        /// 获取所有商店
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/Shop")]
        ITask<List<ShopDto>> GetShops();

        /// <summary>
        /// 根据商店ID，返回商店信息
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet("/api/Shop/{shopId}")]
        ITask<ShopDto> GetShopByShopId(Guid shopId);

        /// <summary>
        /// 修改商店
        /// </summary>
        /// <returns></returns>
        [HttpPut("/api/Shop")]
        ITask<ShopDto> UpdateShop([JsonContent] ShopDto model);
    }
}
