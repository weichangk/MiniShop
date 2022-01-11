using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Model.Enums;
using Orm.Core;
using Orm.Core.Result;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Description("用户信息")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerAbstract
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IMapper mapper, UserManager<IdentityUser> userManager) : base(logger)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [Description("根据商店ID和分页条件获取所有用户")]
        [OperationId("获取用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [HttpGet]
        public async Task<IResultModel> Query()
        {
            var info = await _userManager.GetUsersForClaimAsync(new Claim("ShopId", "cb16cce2-c566-4174-8728-441582c964a9"));
            var list = info.AsQueryable().ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToPagedList(1, 10);
            foreach (var u in list.Item)
            {
                var user = await _userManager.FindByNameAsync(u.UserName);
                var claims = await _userManager.GetClaimsAsync(user);
                foreach (var c in claims)
                {
                    switch (c.Type)
                    {
                        case "rank":                      
                            u.Rank = (EnumRole)Enum.Parse(typeof(EnumRole), c.Value);
                            break;
                        case "":
                            break;
                        case "shopid":
                            u.ShopId = Guid.Parse(c.Value);
                            break;
                        case "storeid":
                            u.StoreId = Guid.Parse(c.Value);
                            break;
                        case "isfreeze":
                            u.IsFreeze = bool.Parse(c.Value);
                            break;
                        case "createdtime":
                            u.CreatedTime = DateTime.Parse(c.Value);
                            break;
                        default:
                            break;
                    }
                }               
            }
            return ResultModel.Success(list);
        }
    }
}
