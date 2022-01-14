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
        [HttpGet("/api/Shop/{id}")]
        ITask<ResultModel<ShopDto>> QueryAsync(Guid id);

        [HttpGet("/api/Shop/QueryByShopId/{shopId}")]
        ITask<ResultModel<ShopDto>> QueryByShopIdAsync(Guid shopId);
 
        [HttpPost("/api/Shop")]
        ITask<ResultModel<ShopCreateDto>> AddAsync([JsonContent] ShopCreateDto model);

        [HttpPut("/api/Shop")]
        ITask<ResultModel<ShopUpdateDto>> UpdateAsync([JsonContent] ShopUpdateDto model);
    }
}
