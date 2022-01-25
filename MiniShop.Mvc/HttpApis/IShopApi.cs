using MiniShop.Dto;
using MiniShop.Mvc.Code;
using System;
using WebApiClient;
using WebApiClient.Attributes;
using Orm.Core.Result;
using Microsoft.AspNetCore.JsonPatch;

namespace MiniShop.Mvc.HttpApis
{
    [MiniShopApi]
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IShopApi : IHttpApi
    {
        [HttpGet("/api/Shop")]
        ITask<ResultModel<ShopDto>> GetByIdAsync(int id);

        [HttpGet("/api/Shop/GetByShopId")]
        ITask<ResultModel<ShopDto>> GetByShopIdAsync(Guid shopId);
 
        [HttpPost("/api/Shop")]
        ITask<ResultModel<ShopCreateDto>> AddAsync([JsonContent] ShopCreateDto model);

        [HttpPut("/api/Shop")]
        ITask<ResultModel<ShopUpdateDto>> PutUpdateAsync([JsonContent] ShopUpdateDto model);

        [HttpPatch("/api/Shop")]
        ITask<ResultModel<UserDto>> PatchUpdateByIdAsync(int id, [JsonContent] JsonPatchDocument<ShopUpdateDto> doc);
    }
}
