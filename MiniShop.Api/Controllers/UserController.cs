using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Api.Ids;
using MiniShop.Dto;
using Orm.Core.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Description("用户信息")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerAbstract
    {
        private readonly UserManager<IdsUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IMapper mapper, UserManager<IdsUser> userManager) : base(logger)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [Description("获取所有用户")]
        [OperationId("获取所有用户")]
        [ResponseCache(Duration = 0)]
        [HttpGet]
        public IResultModel Query()
        {
            var info = _userManager.Users.ToList();
            return ResultModel.Success(_mapper.Map<UserDto>(info));
        }
    }
}
