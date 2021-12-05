using System.Net;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace MiniShop.Mvc.Code
{
    /// <summary>
    /// JWT Headers中加入AccessToken
    /// </summary>
    public class ApiRequestTokenFilter : ApiActionFilterAttribute
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
                context.RequestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {_loginInfo.LoginAccessToken}");
            }
            catch (System.Exception)
            {
                return Task.CompletedTask;
            }
            return base.OnBeginRequestAsync(context);
        }
    }
}
