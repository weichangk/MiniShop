using MiniShop.Mvc.Code;
using WebApiClient;
using WebApiClient.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [SetAccessTokenFilter]
    [JsonReturn]
    public interface IBaseHttpApi : IHttpApi
    {
        //想在通过接口继承添加特性，发现不起作用！！！
    }
}
