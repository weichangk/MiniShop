using System.Net;
using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace MiniShop.Mvc.Code
{
    /// <summary>
    /// JWT Headers中加入AccessToken
    /// </summary>
    public class SetAccessTokenFilter : ApiActionFilterAttribute
    {
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            if (context.Exception != null)
            {
                return Task.CompletedTask;
            }
            try
            {
                var userInfo = context.GetService<IUserInfo>();
                context.RequestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {userInfo.AccessToken}");
            }
            catch (System.Exception)
            {
                return Task.CompletedTask;
            }
            return base.OnBeginRequestAsync(context);
        }
    }
}
