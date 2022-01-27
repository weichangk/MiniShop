using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Orm.Core.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniShop.Api.Code.Filters
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResult(ResultModel.Failed(ModelStateErrorMessage(context.ModelState), (int)HttpStatusCode.BadRequest));
            }
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //if (context.Result is ObjectResult)
            //{
            //    var objectResult = context.Result as ObjectResult;
            //    if (objectResult.Value == null)
            //    {
            //        context.Result = new ObjectResult(ResultModel.Failed(HttpStatusCode.NotFound.ToString(), (int)HttpStatusCode.NotFound));
            //    }
            //}
            //else 
            if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(ResultModel.Failed(HttpStatusCode.NotFound.ToString(), (int)HttpStatusCode.NotFound));
            }
            else if (context.Result is ForbidResult)
            {
                context.Result = new ObjectResult(ResultModel.Failed(HttpStatusCode.Forbidden.ToString(), (int)HttpStatusCode.Forbidden));
            }
            base.OnResultExecuting(context);
        }

        private string ModelStateErrorMessage(ModelStateDictionary modelState)
        {
            var message = string.Join(" | ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return message;
        }
    }

    public class ApiAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Result is ForbidResult)
            { 
            }
        }
    }

    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = BuildExceptionResult(context.Exception);
            base.OnException(context);
        }

        /// <summary>
        /// 包装处理异常格式
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private JsonResult BuildExceptionResult(Exception ex)
        {
            int code = 0;
            string message = "";
            string innerMessage = "";
            //应用程序业务级异常
            if (ex is ApplicationException)
            {
                code = 501;
                message = ex.Message;
            }
            else
            {
                // exception 系统级别异常，不直接明文显示的
                code = 500;
                message = "发生系统级别异常";
                innerMessage = ex.Message;
            }

            if (ex.InnerException != null && ex.Message != ex.InnerException.Message)
                innerMessage += "," + ex.InnerException.Message;

            return new JsonResult(new { code, message, innerMessage });
        }
    }
}
