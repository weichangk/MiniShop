using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MiniShop.Mvc.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiClient.Contexts;

namespace MiniShop.Mvc.Controllers
{
    [Authorize]
    [TokenFilter]
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> _logger;
        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        public string ModelStateErrorMessage(ModelStateDictionary modelState)
        {
            var message = string.Join(" | ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return message;
        }
    }
}
