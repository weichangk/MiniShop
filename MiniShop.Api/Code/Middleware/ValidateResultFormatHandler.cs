using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Orm.Core.Result;

namespace MiniShop.Api.Code.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateResultFormatHandler : IValidateResultFormatHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Format(ResultExecutingContext context)
        {
            var errors = context.ModelState
                .Where(m => m.Value.ValidationState == ModelValidationState.Invalid)
                .Select(m => new Errors
                {
                    Id = m.Key,
                    Msg = m.Value.Errors.Select(n => n.ErrorMessage).Aggregate((x, y) => x + ";" + y)
                }).ToList();

            context.Result = new JsonResult(ResultModel.Failed(errors));
        }
    }
}
