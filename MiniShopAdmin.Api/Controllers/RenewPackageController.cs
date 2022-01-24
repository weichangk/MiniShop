using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShopAdmin.Api.Services;
using Orm.Core.Result;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MiniShopAdmin.Api.Controllers
{
    public class RenewPackageController : ControllerAbstract
    {
        private readonly Lazy<IRenewPackageService> _renewPackageService;
        private readonly Lazy<IRenewPackageCreateService> _renewPackageCreateService;
        private readonly Lazy<IRenewPackageUpdateService> _renewPackageUpdateService;

        public RenewPackageController(ILogger<RenewPackageController> logger,
            Lazy<IRenewPackageService> renewPackageService,
            Lazy<IRenewPackageCreateService> renewPackageCreateService,
            Lazy<IRenewPackageUpdateService> renewPackageUpdateService) : base(logger)
        {
            _renewPackageService = renewPackageService;
            _renewPackageCreateService = renewPackageCreateService;
            _renewPackageUpdateService = renewPackageUpdateService;
        }

        [Description("根据ID获取续费包")]
        [OperationId("根据ID获取续费包")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "id", param = "ID")]
        [HttpGet]
        public async Task<IResultModel> GetById([Required] int id)
        {
            _logger.LogDebug($"根据ID：{id } 获取续费包");
            return await _renewPackageService.Value.GetByIdAsync(id);
        }
    }
}
