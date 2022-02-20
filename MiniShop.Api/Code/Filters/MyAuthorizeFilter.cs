using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace MiniShop.Api.Code.Filters
{
    public class MyAuthorizeFilter : IAuthorizationFilter
    { 
        public void OnAuthorization(AuthorizationFilterContext context) 
        {
            if (context.Filters.Any(it => it is Microsoft.AspNetCore.Mvc.Authorization.IAllowAnonymousFilter))
            {

            }
            else
            {
                var userName = context.HttpContext;

                //if (string.IsNullOrEmpty(userName))
                //{
                //    RedirectResult result = new RedirectResult("~/Login");
                //    context.Result = result;
                //}
                //else
                //{

                //}
            }
        } 
    }
}
