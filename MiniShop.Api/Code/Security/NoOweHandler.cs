using Microsoft.AspNetCore.Authorization;
using MiniShop.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Code.Security
{
    public class NoOweHandler : AuthorizationHandler<NoOweRequirement>
    {
        private readonly IShopService _shopService;
        public NoOweHandler(IShopService shopService)
        {
            _shopService = shopService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NoOweRequirement requirement)
        {
            var value = context.User.Claims.FirstOrDefault(c => c.Type == "shopid").Value;
            var result = (Orm.Core.Result.ResultModel<Dto.ShopDto>)(_shopService.GetByShopIdAsync(Guid.Parse(value)).Result);
            if (result.Data.ValidDate >= DateTime.Now)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
