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
        ITask<ResultModel<ShopDto>> GetByIdAsync(int id);

        [HttpGet("/api/Shop/GetByShopId/{shopId}")]
        ITask<ResultModel<ShopDto>> GetByShopIdAsync(Guid shopId);
 
        [HttpPost("/api/Shop")]
        ITask<ResultModel<ShopCreateDto>> AddAsync([JsonContent] ShopCreateDto model);

        [HttpPut("/api/Shop")]
        ITask<ResultModel<ShopUpdateDto>> UpdateAsync([JsonContent] ShopUpdateDto model);
    }
}
