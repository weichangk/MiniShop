﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model.Enums;
using Orm.Core;
using Orm.Core.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniShop.Api.Controllers
{
    [Description("用户信息")]
    public class UserController : ControllerAbstract
    {
        private readonly Lazy<UserManager<IdentityUser>> _userManager;
        private readonly Lazy<IShopService> _shopService;
        private readonly Lazy<IStoreService> _storeService;

        public UserController(ILogger<UserController> logger, Lazy<IMapper> mapper, Lazy<UserManager<IdentityUser>> userManager,
            Lazy<IShopService> shopService,
            Lazy<IStoreService> storeService) : base(logger, mapper)
        {
            _userManager = userManager;
            _shopService = shopService;
            _storeService = storeService;
        }

        [Description("根据用户名获取用户")]
        [OperationId("获取用户")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "name", param = "用户名")]
        [HttpGet("{name}")]
        public async Task<IResultModel> Query([Required] string name)
        {
            _logger.LogDebug($"根据用户名：{name} 获取用户");
            var data = await _userManager.Value.FindByNameAsync(name);
            if (data == null)
            {
                return ResultModel.NotExists;
            }
            var userDto = _mapper.Value.Map<UserDto>(data);
            await UserDtoSetClaimExtras(userDto);
            return ResultModel.Success(userDto);
        }

        [Description("根据商店ID、分页条件、职位等级获取商店用户分页列表")]
        [OperationId("根据商店ID、分页条件、职位等级获取商店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "rank", param = "职位等级")]
        [HttpGet("GetPage/{pageIndex}/{pageSize}/{shopId}/{rank}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId, EnumRole rank)
        {
            _logger.LogDebug($"根据商店ID：{shopId}  职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取商店的用户分页列表");
            var data = await _userManager.Value.GetUsersForClaimAsync(new Claim("ShopId", shopId.ToString()));
            var dataShopManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopManager"));
            dataShopManager = data.Intersect(dataShopManager).ToList();
            var dataShopAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopAssistant"));
            dataShopAssistant = data.Intersect(dataShopAssistant).ToList();
            var dataStoreManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreManager"));
            dataStoreManager = data.Intersect(dataStoreManager).ToList();
            var dataStoreAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreAssistant"));
            dataStoreAssistant = data.Intersect(dataStoreAssistant).ToList();
            var dataCashier = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "Cashier"));
            dataCashier = data.Intersect(dataCashier).ToList();
            switch (rank)
            {
                case EnumRole.ShopManager:
                    break;
                case EnumRole.ShopAssistant:
                    data = data.Except(dataShopManager).ToList();
                    break;
                case EnumRole.StoreManager:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    break;
                case EnumRole.StoreAssistant:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    break;
                case EnumRole.Cashier:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    data = data.Except(dataStoreAssistant).ToList();
                    break;
                default:
                    break;
            }
            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }
            return ResultModel.Success(userPagedList);
        }

        [Description("根据商店ID、门店ID、分页条件、职位等级获取门店用户分页列表")]
        [OperationId("根据商店ID、门店ID、分页条件、职位等级获取门店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "storeId", param = "门店ID")]
        [Parameters(name = "rank", param = "职位等级")]
        [HttpGet("GetPage/{pageIndex}/{pageSize}/{shopId}/{storeId}/{rank}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId, Guid storeId, EnumRole rank)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 门店ID：{storeId} 职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取门店的用户分页列表");
            var dataByShopId = await _userManager.Value.GetUsersForClaimAsync(new Claim("ShopId", shopId.ToString()));
            var dataByStoreId = await _userManager.Value.GetUsersForClaimAsync(new Claim("StoreId", storeId.ToString()));
            var data = dataByShopId.Intersect(dataByStoreId).ToList();
            var dataShopManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopManager"));
            dataShopManager = data.Intersect(dataShopManager).ToList();
            var dataShopAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopAssistant"));
            dataShopAssistant = data.Intersect(dataShopAssistant).ToList();
            var dataStoreManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreManager"));
            dataStoreManager = data.Intersect(dataStoreManager).ToList();
            var dataStoreAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreAssistant"));
            dataStoreAssistant = data.Intersect(dataStoreAssistant).ToList();
            var dataCashier = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "Cashier"));
            dataCashier = data.Intersect(dataCashier).ToList();
            switch (rank)
            {
                case EnumRole.ShopManager:
                    break;
                case EnumRole.ShopAssistant:
                    data = data.Except(dataShopManager).ToList();
                    break;
                case EnumRole.StoreManager:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    break;
                case EnumRole.StoreAssistant:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    break;
                case EnumRole.Cashier:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    data = data.Except(dataStoreAssistant).ToList();
                    break;
                default:
                    break;
            }

            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }
            return ResultModel.Success(userPagedList);
        }

        [Description("根据商店ID、分页条件、职位等级，和职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [OperationId("根据商店ID、分页条件、职位等级，和职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "rank", param = "职位等级")]
        [Parameters(name = "queryRank", param = "职位查询")]
        [Parameters(name = "queryName", param = "用户名查询")]
        [Parameters(name = "queryPhone", param = "手机号查询")]
        [HttpGet("GetPageWhereQuery/{pageIndex}/{pageSize}/{shopId}/{rank}/{queryRank}/{queryName}/{queryPhone}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId, EnumRole rank, EnumRole? queryRank, string queryName, string queryPhone)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 模糊查询条件： 职位：{queryRank} 用户名：{queryName} 手机号：{queryPhone} 获取商店的用户分页列表");
            var data = await _userManager.Value.GetUsersForClaimAsync(new Claim("ShopId", shopId.ToString()));

            var dataShopManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopManager"));
            dataShopManager = data.Intersect(dataShopManager).ToList();
            var dataShopAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopAssistant"));
            dataShopAssistant = data.Intersect(dataShopAssistant).ToList();
            var dataStoreManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreManager"));
            dataStoreManager = data.Intersect(dataStoreManager).ToList();
            var dataStoreAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreAssistant"));
            dataStoreAssistant = data.Intersect(dataStoreAssistant).ToList();
            var dataCashier = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "Cashier"));
            dataCashier = data.Intersect(dataCashier).ToList();
            switch (rank)
            {
                case EnumRole.ShopManager:
                    break;
                case EnumRole.ShopAssistant:
                    data = data.Except(dataShopManager).ToList();
                    break;
                case EnumRole.StoreManager:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    break;
                case EnumRole.StoreAssistant:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    break;
                case EnumRole.Cashier:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    data = data.Except(dataStoreAssistant).ToList();
                    break;
                default:
                    break;
            }

            if (queryRank != null)
            {
                var dataQueryRank = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", queryRank.ToString()));
                data = data.Intersect(dataQueryRank).ToList();
            }
            if (!string.IsNullOrEmpty(queryName))
            {
                queryName = System.Web.HttpUtility.UrlDecode(queryName);
                data = data.Where(u => u.UserName.Contains(queryName)).ToList();
            }
            if (!string.IsNullOrEmpty(queryPhone))
            {
                queryPhone = System.Web.HttpUtility.UrlDecode(queryPhone);
                data = data.Where(u => u.UserName.Contains(queryPhone)).ToList();
            }

            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }

            return ResultModel.Success(userPagedList);
        }

        [Description("根据商店ID、分页条件、职位等级，和职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [OperationId("根据商店ID、分页条件、职位等级，和职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "rank", param = "职位等级")]
        [Parameters(name = "queryStore", param = "职位门店")]
        [Parameters(name = "queryRank", param = "职位查询")]
        [Parameters(name = "queryName", param = "用户名查询")]
        [Parameters(name = "queryPhone", param = "手机号查询")]
        [HttpGet("GetPageWhereQuery/{pageIndex}/{pageSize}/{shopId}/{rank}/{queryStore}/{queryRank}/{queryName}/{queryPhone}")]
        public async Task<IResultModel> Query([Required] int pageIndex, int pageSize, Guid shopId, EnumRole rank, Guid? queryStore, EnumRole? queryRank, string queryName, string queryPhone)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 模糊查询条件： 门店：{queryStore} 职位：{queryRank} 用户名：{queryName} 手机号：{queryPhone} 获取商店的用户分页列表");
            var data = await _userManager.Value.GetUsersForClaimAsync(new Claim("ShopId", shopId.ToString()));

            var dataShopManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopManager"));
            dataShopManager = data.Intersect(dataShopManager).ToList();
            var dataShopAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "ShopAssistant"));
            dataShopAssistant = data.Intersect(dataShopAssistant).ToList();
            var dataStoreManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreManager"));
            dataStoreManager = data.Intersect(dataStoreManager).ToList();
            var dataStoreAssistant = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "StoreAssistant"));
            dataStoreAssistant = data.Intersect(dataStoreAssistant).ToList();
            var dataCashier = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", "Cashier"));
            dataCashier = data.Intersect(dataCashier).ToList();
            switch (rank)
            {
                case EnumRole.ShopManager:
                    break;
                case EnumRole.ShopAssistant:
                    data = data.Except(dataShopManager).ToList();
                    break;
                case EnumRole.StoreManager:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    break;
                case EnumRole.StoreAssistant:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    break;
                case EnumRole.Cashier:
                    data = data.Except(dataShopManager).ToList();
                    data = data.Except(dataShopAssistant).ToList();
                    data = data.Except(dataStoreManager).ToList();
                    data = data.Except(dataStoreAssistant).ToList();
                    break;
                default:
                    break;
            }

            if (queryStore != null)
            {
                var dataByStoreId = await _userManager.Value.GetUsersForClaimAsync(new Claim("StoreId", queryStore.ToString()));
                data = data.Intersect(dataByStoreId).ToList();
            }
            if (queryRank != null)
            {
                var dataQueryRank = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", queryRank.ToString()));
                data = data.Intersect(dataQueryRank).ToList();
            }
            if (!string.IsNullOrEmpty(queryName))
            {
                queryName = System.Web.HttpUtility.UrlDecode(queryName);
                data = data.Where(u => u.UserName.Contains(queryName)).ToList();
            }
            if (!string.IsNullOrEmpty(queryPhone))
            {
                queryPhone = System.Web.HttpUtility.UrlDecode(queryPhone);
                data = data.Where(u => u.UserName.Contains(queryPhone)).ToList();
            }

            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }

            return ResultModel.Success(userPagedList);
        }

        [Description("根据用户名删除用户")]
        [OperationId("删除用户")]
        [Parameters(name = "name", param = "用户名")]
        [HttpDelete("{name}")]
        public async Task<IResultModel> Delete([Required] string name)
        {
            _logger.LogDebug("删除用户");
            var data = await _userManager.Value.FindByNameAsync(name);
            if (data == null)
            {
                _logger.LogError($"删除错误：用户：{name} 不存在");
                return ResultModel.NotExists;
            }
            else
            {
                var result = await _userManager.Value.DeleteAsync(data);
                if (!result.Succeeded)
                {
                    _logger.LogError($"删除错误：{result.Errors.FirstOrDefault().Description}");
                    return ResultModel.Failed($"删除错误：用户：{name} 删除失败", 500);
                }
            }
            return ResultModel.Success();
        }

        [Description("根据用户名批量删除用户")]
        [OperationId("删除用户")]
        [Parameters(name = "names", param = "用户名列表")]
        [HttpDelete("BatchDelete")]
        public async Task<IResultModel> Delete([FromBody] List<string> names)
        {
            _logger.LogDebug("批量删除用户");
            foreach (var name in names)
            {
                var data = await _userManager.Value.FindByNameAsync(name);
                if (data == null)
                {
                    _logger.LogError($"删除错误：用户：{name} 不存在");
                    return ResultModel.NotExists;
                }
                else
                {
                    var result = await _userManager.Value.DeleteAsync(data);
                    if (!result.Succeeded)
                    {
                        _logger.LogError($"删除错误：{result.Errors.FirstOrDefault().Description}");
                        return ResultModel.Failed("删除错误：用户：{name} 删除失败", 500);
                    }
                }
            }
            return ResultModel.Success();
        }

        [Description("添加用户，成功返回用户信息")]
        [OperationId("添加用户")]
        [HttpPost]
        public async Task<IResultModel> Add([FromBody] UserCreateDto model)
        {
            _logger.LogDebug("添加用户");
            if (ModelState.IsValid)
            {
                var user = await _userManager.Value.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    _logger.LogError($"添加错误：用户：{model.UserName} 已存在");
                    return ResultModel.HasExists;
                }

                user = new IdentityUser
                {
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                };

                var result = await _userManager.Value.CreateAsync(user, model.PassWord);
                if (!result.Succeeded)
                {
                    _logger.LogError($"添加错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"添加错误：{result.Errors.First().Description}");
                }

                result = await _userManager.Value.AddToRolesAsync(user, new List<string> { model.Rank.ToString() });
                if (!result.Succeeded)
                {
                    _logger.LogError($"添加错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"添加错误：{result.Errors.First().Description}");
                }

                result = await AddUserClaimExtras(user, model);
                if (!result.Succeeded)
                {
                    _logger.LogError($"添加错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"添加错误：{result.Errors.First().Description}");
                }

                var data = await _userManager.Value.FindByNameAsync(model.UserName);
                var userDto = _mapper.Value.Map<UserDto>(data);
                return ResultModel.Success(userDto);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

        [Description("Put修改用户，成功返回用户信息")]
        [OperationId("修改用户")]
        [HttpPut]
        public async Task<IResultModel> Update([FromBody] UserUpdateDto model)
        {
            _logger.LogDebug("修改用户");
            if (ModelState.IsValid)
            {
                var user = await _userManager.Value.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    _logger.LogError($"修改错误：用户：{model.UserName} 不存在");
                    return ResultModel.NotExists;
                }
                user = _mapper.Value.Map<IdentityUser>(model);
                var result = await _userManager.Value.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError($"修改错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"修改错误：{result.Errors.First().Description}");
                }

                result = await UpdateUserClaimExtras(user, model);
                if (!result.Succeeded)
                {
                    _logger.LogError($"修改错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"修改错误：{result.Errors.First().Description}");
                }

                var data = await _userManager.Value.FindByNameAsync(model.UserName);
                var userDto = _mapper.Value.Map<UserDto>(data);
                await UserDtoSetClaimExtras(userDto);
                return ResultModel.Success(userDto);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

        [Description("Patch修改用户，成功返回用户信息")]
        [OperationId("修改用户")]
        [Parameters(name = "name", param = "用户名")]
        [HttpPatch("{name}")]
        public async Task<IResultModel> Update([FromRoute] string name, [FromBody] JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            _logger.LogDebug("修改用户");
            if (ModelState.IsValid)
            {
                var user = await _userManager.Value.FindByNameAsync(name);
                if (user == null)
                {
                    _logger.LogError($"修改错误：用户：{name} 不存在");
                    return ResultModel.NotExists;
                }
                var modelRouteToPatch = _mapper.Value.Map<UserUpdateDto>(user);
                await UserUpdateDtoSetClaimExtras(modelRouteToPatch);
                patchDocument.ApplyTo(modelRouteToPatch);
                _mapper.Value.Map(modelRouteToPatch, user);
                var result = await _userManager.Value.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError($"修改错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"修改错误：{result.Errors.First().Description}");
                }

                result = await UpdateUserClaimExtras(user, modelRouteToPatch);
                if (!result.Succeeded)
                {
                    _logger.LogError($"修改错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"修改错误：{result.Errors.First().Description}");
                }

                var data = await _userManager.Value.FindByNameAsync(name);
                var userDto = _mapper.Value.Map<UserDto>(data);
                await UserDtoSetClaimExtras(userDto);
                return ResultModel.Success(userDto);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

        private async Task UserDtoSetClaimExtras(UserDto dto)
        {
            var user = await _userManager.Value.FindByNameAsync(dto.UserName);
            var claims = await _userManager.Value.GetClaimsAsync(user);
            foreach (var c in claims)
            {
                switch (c.Type)
                {
                    case "rank":
                        dto.Rank = (EnumRole)Enum.Parse(typeof(EnumRole), c.Value);
                        break;
                    case "shopid":
                        dto.ShopId = Guid.Parse(c.Value);
                        var shop = await _shopService.Value.GetByShopIdAsync(dto.ShopId);
                        dto.ShopName = ((IResultModel<ShopDto>)shop).Data.Name;
                        break;
                    case "storeid":
                        dto.StoreId = Guid.Parse(c.Value);
                        var store = await _storeService.Value.GetByStoreIdAsync(dto.StoreId);
                        dto.StoreName = ((IResultModel<StoreDto>)store).Data.Name;
                        break;
                    case "isfreeze":
                        dto.IsFreeze = bool.Parse(c.Value);
                        break;
                    case "createdtime":
                        dto.CreatedTime = DateTime.Parse(c.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task<IdentityResult> AddUserClaimExtras(IdentityUser user, UserCreateDto model)
        {
            var result = await _userManager.Value.AddClaimsAsync(user, new Claim[]{
                            new Claim("rank", model.Rank.ToString()),
                            new Claim("shopid", model.ShopId.ToString()),
                            new Claim("storeid", model.StoreId.ToString()),
                            new Claim("isfreeze", "false"),
                            new Claim("createdtime", DateTime.Now.ToString()),
                        });
            return result;
        }

        private async Task UserUpdateDtoSetClaimExtras(UserUpdateDto dto)
        {
            var user = await _userManager.Value.FindByNameAsync(dto.UserName);
            var claims = await _userManager.Value.GetClaimsAsync(user);
            foreach (var c in claims)
            {
                switch (c.Type)
                {
                    case "rank":
                        dto.Rank = (EnumRole)Enum.Parse(typeof(EnumRole), c.Value);
                        break;
                    case "":
                        break;
                    case "isfreeze":
                        dto.IsFreeze = bool.Parse(c.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task<IdentityResult> UpdateUserClaimExtras(IdentityUser user, UserUpdateDto model)
        {
            var userDto = _mapper.Value.Map<UserDto>(user);
            await UserDtoSetClaimExtras(userDto);
            var result =  await _userManager.Value.ReplaceClaimAsync(user, new Claim("rank", userDto.Rank.ToString()), new Claim("rank", model.Rank.ToString()));
            if (!result.Succeeded)
            {
                return result;
            }

            var claims = await _userManager.Value.GetClaimsAsync(user);
            if (claims.FirstOrDefault(c => c.Type == "rank")?.Value != model.Rank.ToString())
            {
                var roles = await _userManager.Value.GetRolesAsync(user);
                result = await _userManager.Value.RemoveFromRolesAsync(user, roles);
                if (!result.Succeeded)
                {
                    return result;
                }
                result = await _userManager.Value.AddToRoleAsync(user, model.Rank.ToString());
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            await _userManager.Value.ReplaceClaimAsync(user, new Claim("isfreeze", userDto.IsFreeze.ToString()), new Claim("isfreeze", model.IsFreeze.ToString()));
            if (!result.Succeeded)
            {
                return result;
            }
            return result;
        }

    }
}
