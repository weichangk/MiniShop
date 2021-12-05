using MiniShop.Model;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("/api/User/CreateDefaultShopAndUser/{userName}/{phone}/{email}/{role}")]
        ITask<User> CreateDefaultShopAndUser(string userName, string phone, string email, string role);
    }
}
