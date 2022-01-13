using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Web;

namespace MiniShop.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ControllerAbstract : ControllerBase
    {
        /*
         * 有个坑，在该基类定义公共方法如果用public修饰时Swagger文档保存报错：Failed to load API definition！！！用protected就可以。。。
         */

        protected readonly ILogger<ControllerAbstract> _logger;

        protected readonly Lazy<IMapper> _mapper;


        public ControllerAbstract(ILogger<ControllerAbstract> logger, Lazy<IMapper> mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        protected string ModelStateErrorMessage(ModelStateDictionary modelState)
        {
            var message = string.Join(" | ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return message;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected IActionResult ExportExcel(string filePath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", HttpUtility.UrlEncode(fileName), true);
        }
    }
}
