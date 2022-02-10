using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Orm.Core.Result;
using System.Linq;
using System.Net;

namespace MiniShop.Api.Code.Filters
{
    public class ActionFilter : ActionFilterAttribute
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
            base.OnResultExecuting(context);
        }

        private string ModelStateErrorMessage(ModelStateDictionary modelState)
        {
            var message = string.Join(" | ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return message;
        }
    }
}
