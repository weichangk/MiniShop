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
                context.RequestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), $"Bearer {LoginInfos.AccessToken}");
            }
            catch (System.Exception)
            {
                return Task.CompletedTask;
            }
            return base.OnBeginRequestAsync(context);
        }
    }
}
