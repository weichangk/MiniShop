using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Web;

namespace MiniShop.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ControllerAbstract : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly ILogger<ControllerAbstract> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ControllerAbstract(ILogger<ControllerAbstract> logger)
        {
            _logger = logger;
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
