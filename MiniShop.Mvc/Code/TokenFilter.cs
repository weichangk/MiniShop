using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace MiniShop.Mvc.Code
{
    /// <summary>
    /// JWT Headers中加入AccessToken
    /// </summary>
    public class TokenFilter : ApiActionFilterAttribute
    {
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            if (context.Exception != null)
            {
                return Task.CompletedTask;
            }
            try
            {
                var _loginInfo = context.GetService<ILoginInfo>();
                context.RequestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {_loginInfo.AccessToken}");
            }
            catch (System.Exception)
            {
                return Task.CompletedTask;
            }
            return base.OnBeginRequestAsync(context);
        }
    }
}
