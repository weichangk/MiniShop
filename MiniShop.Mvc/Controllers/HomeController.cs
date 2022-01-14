using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.Mvc.Code;
using MiniShop.Mvc.HttpApis;
using MiniShop.Mvc.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IShopApi _shopApi;
        private readonly IStoreApi _storeApi;
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IUserInfo userInfo,
            IShopApi shopApi, IStoreApi storeApi) : base(logger, mapper, userInfo)
        {
            _shopApi = shopApi;
            _storeApi = storeApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SetShopStoreDefaultInfo();
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Privacy()
        {
            ViewBag.IdToken = _userInfo.IdToken;
            ViewBag.AccessToken = _userInfo.AccessToken;
            ViewBag.RefreshToken = _userInfo.RefreshToken;
            ViewBag.UserName = _userInfo.UserName;
            ViewBag.Rank = _userInfo.Rank;
            ViewBag.ShopId = _userInfo.ShopId;
            ViewBag.StoreId = _userInfo.StoreId;
            ViewBag.PhoneNumber = _userInfo.PhoneNumber;
            ViewBag.Email = _userInfo.Email;
            ViewBag.IsFreeze = _userInfo.IsFreeze;
            ViewBag.CreatedTime = _userInfo.CreatedTime;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IActionResult> SetShopStoreDefaultInfo()
        {
            if (_userInfo.Rank == Model.Enums.EnumRole.ShopManager)
            {
                var queryShop = await _shopApi.QueryByShopIdAsync(_userInfo.ShopId);
                if (queryShop.Data == null)
                {
                    ShopCreateDto shopCreateDto = new ShopCreateDto
                    {
                        Id = _userInfo.ShopId,
                        Name = $"{_userInfo.UserName}的商店",
                        Contacts = _userInfo.UserName,
                        Phone = _userInfo.PhoneNumber,
                        Email = _userInfo.Email,
                    };
                    var addShop =  await _shopApi.AddAsync(shopCreateDto);
                    if (!addShop.Success)
                    {
                        //应该重定向到错误页
                        return Json(new Result() { Success = addShop.Success, Msg = addShop.Msg, status = addShop.Status });
                    }
                }

                var queryStore = await _storeApi.QueryAsync(_userInfo.StoreId);
                if (queryStore.Data == null)
                {
                    StoreCreateDto storeCreateDto = new StoreCreateDto
                    {
                        Id = _userInfo.StoreId,
                        ShopId =_userInfo.ShopId,
                        Name = $"{_userInfo.UserName}的门店",
                        Contacts = _userInfo.UserName,
                        Phone =_userInfo.PhoneNumber,               
                    };
                    var addStore = await _storeApi.AddAsync(storeCreateDto);
                    if (!addStore.Success)
                    {
                        //应该重定向到错误页
                        return Json(new Result() { Success = addStore.Success, Msg = addStore.Msg, status = addStore.Status });
                    }
                }
            }

            return Json(new Result() { Success = true });
        }

    }
}
