using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MiniShop.Api.Code.Attributes;
using System;
using System.Linq;
using System.Web;

namespace MiniShop.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [ValidateResultFormat]
    [ApiController]
    public class ControllerAbstract : ControllerBase
    {
        /* 坑坑坑
         * 
         * 在该基类定义公共方法如果用public修饰时Swagger文档保存报错：Failed to load API definition！！！用protected就可以。。。 应该是public都需要带http请求方法才行？
         * 
         * 使用Identity时，对于未认证访问添加Authorize认证过滤器的api时返回的是404！！！ 需要使用[Authorize(AuthenticationSchemes = "Bearer")] 
         * 参考https://stackoverflow.com/questions/49009977/getting-a-404-when-accessing-an-authorize-controller-when-authenticated
         * 
         */



        protected readonly ILogger<ControllerAbstract> _logger;

        protected readonly Lazy<IMapper> _mapper;


        public ControllerAbstract(ILogger<ControllerAbstract> logger, Lazy<IMapper> mapper)
        {
            _logger = logger;
            _mapper = mapper;
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
