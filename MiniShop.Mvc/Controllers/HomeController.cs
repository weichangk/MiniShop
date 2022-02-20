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
using System.Threading.Tasks;

namespace MiniShop.Mvc.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IShopApi _shopApi;
        private readonly IStoreApi _storeApi;
        private readonly ICategorieApi _categorieApi;
        private readonly IUnitApi _unitApi;
        private readonly ISupplierApi _supplierApi;
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IUserInfo userInfo,
            IShopApi shopApi,
            IStoreApi storeApi,
            ICategorieApi categorieApi,
            IUnitApi unitApi,
            ISupplierApi supplierApi) : base(logger, mapper, userInfo)
        {
            _shopApi = shopApi;
            _storeApi = storeApi;
            _categorieApi = categorieApi;
            _unitApi = unitApi;
            _supplierApi = supplierApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await SetShopDefaultInfo();
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
            ViewBag.RefreshToken = HttpContext.GetTokenAsync(Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectParameterNames.RefreshToken).Result;

            return View();
        }

        private async Task<IActionResult> SetShopDefaultInfo()
        {
            //if (_userInfo.Rank == Model.Enums.EnumRole.ShopManager)
            //{
            var queryShop = await ExecuteApiResultModelAsync(() => { return _shopApi.GetByShopIdAsync(_userInfo.ShopId); });
            if (!queryShop.Success)
            {
                return RedirectToAction("Error", "Error", new { statusCode = queryShop.Status, errorMsg = queryShop.Msg });
            }
            if (queryShop.Data == null)
            {
                ShopCreateDto shopCreateDto = new ShopCreateDto
                {
                    ShopId = _userInfo.ShopId,
                    Name = $"{_userInfo.UserName}的商店",
                    Contacts = _userInfo.UserName,
                    Phone = _userInfo.PhoneNumber,
                    Email = _userInfo.Email,
                    ValidDate = System.DateTime.Now.AddDays(7),
                };
                var addShop = await ExecuteApiResultModelAsync(() => { return _shopApi.AddAsync(shopCreateDto); });
                if (!addShop.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = addShop.Status, errorMsg = addShop.Msg });
                }
            }

            var queryStore = await ExecuteApiResultModelAsync(() => { return _storeApi.GetByStoreIdAsync(_userInfo.StoreId); });
            if (!queryStore.Success)
            {
                return RedirectToAction("Error", "Error", new { statusCode = queryShop.Status, errorMsg = queryShop.Msg });
            }
            if (queryStore.Data == null)
            {
                StoreCreateDto storeCreateDto = new StoreCreateDto
                {
                    StoreId = _userInfo.StoreId,
                    ShopId = _userInfo.ShopId,
                    Name = $"{_userInfo.UserName}的门店",
                    Contacts = _userInfo.UserName,
                    Phone = _userInfo.PhoneNumber,
                };
                var addStore = await ExecuteApiResultModelAsync(() => { return _storeApi.AddAsync(storeCreateDto); });
                if (!addStore.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = addStore.Status, errorMsg = addStore.Msg });
                }
            }

            var queryCategorie = await ExecuteApiResultModelAsync(() => { return _categorieApi.GetByCodeOnShop(_userInfo.ShopId, 0); });
            if (!queryCategorie.Success)
            {
                return RedirectToAction("Error", "Error", new { statusCode = queryShop.Status, errorMsg = queryShop.Msg });
            }
            if (queryCategorie.Data == null)
            {
                CategorieCreateDto categorieCreateDto = new CategorieCreateDto
                {
                    ShopId = _userInfo.ShopId,
                    Code = 0,
                    Name = "无类别",
                    Level = 0,
                    ParentCode = 0,
                };
                var addCategorie = await ExecuteApiResultModelAsync(() => { return _categorieApi.AddAsync(categorieCreateDto); });
                if (!addCategorie.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = addCategorie.Status, errorMsg = addCategorie.Msg });
                }
            }

            var queryUnit = await ExecuteApiResultModelAsync(() => { return _unitApi.GetByCodeOnShop(_userInfo.ShopId, 0); });
            if (queryUnit.Data == null)
            {
                UnitCreateDto unitCreateDto = new UnitCreateDto
                {
                    ShopId = _userInfo.ShopId,
                    Code = 0,
                    Name = "无单位",
                };
                var addUnit = await ExecuteApiResultModelAsync(() => { return _unitApi.AddAsync(unitCreateDto); });
                if (!addUnit.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = addUnit.Status, errorMsg = addUnit.Msg });
                }
            }

            var querySupplier = await ExecuteApiResultModelAsync(() => { return _supplierApi.GetByCodeOnShop(_userInfo.ShopId, 0); });
            if (querySupplier.Data == null)
            {
                SupplierCreateDto supplierCreateDto = new SupplierCreateDto
                {
                    ShopId = _userInfo.ShopId,
                    Code = 0,
                    Name = "自采购供应商",
                    Contacts = "无",
                    Phone = "18211111111",
                };
                var addSupplier = await ExecuteApiResultModelAsync(() => { return _supplierApi.AddAsync(supplierCreateDto); });
                if (!addSupplier.Success)
                {
                    return RedirectToAction("Error", "Error", new { statusCode = addSupplier.Status, errorMsg = addSupplier.Msg });
                }
            }
            //}

            return Json(new Result() { Success = true });
        }

    }
}
