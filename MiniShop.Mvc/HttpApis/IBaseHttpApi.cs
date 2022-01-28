using Microsoft.AspNetCore.Mvc;
using MiniShop.Mvc.Code;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace MiniShop.Mvc.HttpApis
{
    [SetAccessToken]
    [JsonReturn]
    public interface IBaseHttpApi : IHttpApi
    {
        //想在通过接口继承添加特性，发现不起作用！！！
    }
}
