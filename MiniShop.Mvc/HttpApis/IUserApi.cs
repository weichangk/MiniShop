using MiniShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [JsonReturn]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("/api/User/UserLogin/{userName}/{phone}/{email}/{role}")]
        ITask<User> UserLogin(string userName, string phone, string email, string role);
    }
}
