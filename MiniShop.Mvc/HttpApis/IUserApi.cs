using MiniShop.Dto;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("/api/User/CreateDefaultShopAndUser/{userName}/{phone}/{email}/{role}")]
        ITask<UserInfoDto> CreateDefaultShopAndUser(string userName, string phone, string email, string role);
    }
}
