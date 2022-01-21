using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model.Code;
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
        [OperationId("根据用户名获取用户")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "name", param = "用户名")]
        [HttpGet]
        public async Task<IResultModel> GetByName([Required] string name)
        {
            _logger.LogDebug($"根据用户名：{name} 获取用户");
            var data = await _userManager.Value.FindByNameAsync(name);
            if (data == null)
            {
                return ResultModel.Failed($"用户：{name} 不存在");
            }
            var userDto = _mapper.Value.Map<UserDto>(data);
            await UserDtoSetClaimExtras(userDto);
            return ResultModel.Success(userDto);
        }

        [Description("根据商店ID、分页条件、请求者职位等级获取商店用户分页列表")]
        [OperationId("根据商店ID、分页条件、请求者职位等级获取商店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [HttpGet("GetPageByRankOnShop")]
        public async Task<IResultModel> GetPageByRankOnShop([Required] int pageIndex, int pageSize, Guid shopId, EnumRole rank)
        {
            _logger.LogDebug($"根据商店ID：{shopId}  职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取商店的用户分页列表");
            var data = await _userManager.Value.GetUsersForClaimAsync(new Claim("shopid", shopId.ToString()));
            data = await IdentityUsersFilterByRank(data, rank);
            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }
            return ResultModel.Success(userPagedList);
        }

        [Description("根据商店ID、门店ID、分页条件、请求者职位等级获取门店用户分页列表")]
        [OperationId("根据商店ID、门店ID、分页条件、请求者职位等级获取门店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "storeId", param = "门店ID")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [HttpGet("GetPageByRankOnStore")]
        public async Task<IResultModel> GetPageByRankOnStore([Required] int pageIndex, int pageSize, Guid shopId, Guid storeId, EnumRole rank)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 门店ID：{storeId} 职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 获取门店的用户分页列表");
            var dataByShopId = await _userManager.Value.GetUsersForClaimAsync(new Claim("shopid", shopId.ToString()));
            var dataByStoreId = await _userManager.Value.GetUsersForClaimAsync(new Claim("storeid", storeId.ToString()));
            var data = dataByShopId.Intersect(dataByStoreId).ToList();
            data = (await IdentityUsersFilterByRank(data, rank)).ToList();
            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }
            return ResultModel.Success(userPagedList);
        }

        [Description("根据商店ID、分页条件、请求者职位等级，和职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [OperationId("根据商店ID、分页条件、请求者职位等级，和职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [Parameters(name = "queryRank", param = "职位查询")]
        [Parameters(name = "queryName", param = "用户名查询")]
        [Parameters(name = "queryPhone", param = "手机号查询")]
        [HttpGet("GetPageByRankOnShopWhereQueryRankOrNameOrPhone")]
        public async Task<IResultModel> GetPageByRankOnShopWhereQueryRankOrNameOrPhone([Required] int pageIndex, int pageSize, Guid shopId, EnumRole rank, EnumRole? queryRank, string queryName, string queryPhone)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 模糊查询条件： 职位：{queryRank} 用户名：{queryName} 手机号：{queryPhone} 获取商店的用户分页列表");
            var data = await _userManager.Value.GetUsersForClaimAsync(new Claim("shopid", shopId.ToString()));
            data = await IdentityUsersFilterByRank(data, rank);

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
                data = data.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(queryPhone)).ToList();
            }

            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }

            return ResultModel.Success(userPagedList);
        }

        [Description("根据商店ID、分页条件、请求者职位等级，和门店、职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [OperationId("根据商店ID、分页条件、请求者职位等级，和门店、职位、用户名、手机号模糊查询条件获取商店用户分页列表")]
        [ResponseCache(Duration = 0)]
        [Parameters(name = "pageIndex", param = "索引页")]
        [Parameters(name = "pageSize", param = "单页条数")]
        [Parameters(name = "shopId", param = "商店ID")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [Parameters(name = "queryStore", param = "职位门店")]
        [Parameters(name = "queryRank", param = "职位查询")]
        [Parameters(name = "queryName", param = "用户名查询")]
        [Parameters(name = "queryPhone", param = "手机号查询")]
        [HttpGet("GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhone")]
        public async Task<IResultModel> GetPageByRankOnShopWhereQueryStoreOrRankOrNameOrPhone([Required] int pageIndex, int pageSize, Guid shopId, EnumRole rank, Guid? queryStore, EnumRole? queryRank, string queryName, string queryPhone)
        {
            _logger.LogDebug($"根据商店ID：{shopId} 职位等级：{rank} 分页条件：索引页{pageIndex} 单页条数{pageSize} 模糊查询条件： 门店：{queryStore} 职位：{queryRank} 用户名：{queryName} 手机号：{queryPhone} 获取商店的用户分页列表");
            var data = await _userManager.Value.GetUsersForClaimAsync(new Claim("shopid", shopId.ToString()));
            data = await IdentityUsersFilterByRank(data, rank);

            if (queryStore != null)
            {
                var dataByStoreId = await _userManager.Value.GetUsersForClaimAsync(new Claim("storeid", queryStore.ToString()));
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
                data = data.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(queryPhone)).ToList();
            }

            var userPagedList = data.AsQueryable().ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedList(pageIndex, pageSize);
            foreach (var u in userPagedList.Item)
            {
                await UserDtoSetClaimExtras(u);
            }

            return ResultModel.Success(userPagedList);
        }

        [Description("根据请求者职位等级、用户名删除用户")]
        [OperationId("根据请求者职位等级、用户名删除用户")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [Parameters(name = "name", param = "用户名")]
        [HttpDelete]
        public async Task<IResultModel> DeleteByName([Required] EnumRole rank, string name)
        {
            _logger.LogDebug("删除用户");
            var user = await _userManager.Value.FindByNameAsync(name);
            if (user == null)
            {
                _logger.LogError($"用户删除错误：{name} 不存在");
                return ResultModel.Failed($"用户删除错误：{name} 不存在");
            }
            else
            {
                var verification = await DeleteUserBeforeVerification(rank, user);
                if (!verification.Success)
                {
                    return verification;
                }

                var result = await _userManager.Value.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError($"用户删除错误：{result.Errors.FirstOrDefault().Description}");
                    return ResultModel.Failed($"用户删除错误：{name} 删除失败", (int)HttpStatusCode.InternalServerError);
                }
            }
            return ResultModel.Success();
        }

        [Description("根据请求者职位等级、用户名列表批量删除用户")]
        [OperationId("根据请求者职位等级、用户名列表批量删除用户")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [Parameters(name = "names", param = "用户名列表")]
        [HttpDelete("BatchDelete")]
        public async Task<IResultModel> BatchDeleteByNames([Required] EnumRole rank, [FromBody] List<string> names)
        {
            _logger.LogDebug("批量删除用户");
            List<IdentityUser> users = new List<IdentityUser>();
            foreach (var name in names)
            {
                var user = await _userManager.Value.FindByNameAsync(name);
                if (user == null)
                {
                    _logger.LogError($"用户删除错误：{name} 不存在");
                    return ResultModel.Failed($"用户删除错误：{name} 不存在");
                }
                var verification = await DeleteUserBeforeVerification(rank, user);
                if (!verification.Success)
                {
                    return verification;
                }
                users.Add(user);
            }
            foreach (var u in users)
            {
                var result = await _userManager.Value.DeleteAsync(u);
                if (!result.Succeeded)
                {
                    _logger.LogError($"用户删除错误：{result.Errors.FirstOrDefault().Description}");
                    return ResultModel.Failed("用户删除错误：{name} 删除失败", (int)HttpStatusCode.InternalServerError);
                }
            }
            return ResultModel.Success();
        }

        [Description("根据请求者职位等级新增用户，成功返回用户信息")]
        [OperationId("根据请求者职位等级新增用户")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [HttpPost]
        public async Task<IResultModel> Add([Required] EnumRole rank, [FromBody] UserCreateDto model)
        {
            _logger.LogDebug("新增用户");
            if (ModelState.IsValid)
            {
                var user = await _userManager.Value.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    _logger.LogError($"用户新增错误：{model.UserName} 已存在");
                    return ResultModel.Failed($"用户新增错误：{model.UserName} 已存在");
                }

                var verification = await AddUserBeforeVerification(rank, model);
                if (!verification.Success)
                {
                    return verification;
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
                    _logger.LogError($"用户新增错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"用户新增错误：{result.Errors.First().Description}");
                }

                result = await _userManager.Value.AddToRolesAsync(user, new List<string> { model.Rank.ToString() });
                if (!result.Succeeded)
                {
                    _logger.LogError($"用户新增错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"用户新增错误：{result.Errors.First().Description}");
                }

                result = await AddUserClaimExtras(user, model);
                if (!result.Succeeded)
                {
                    _logger.LogError($"用户新增错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"用户新增错误：{result.Errors.First().Description}");
                }

                var data = await _userManager.Value.FindByNameAsync(model.UserName);
                var userDto = _mapper.Value.Map<UserDto>(data);
                return ResultModel.Success(userDto);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

        [Description("根据请求者职位等级Put修改用户，成功返回用户信息")]
        [OperationId("根据请求者职位等级Put修改用户")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [HttpPut]
        public async Task<IResultModel> PutUpdate([Required] EnumRole rank, [FromBody] UserUpdateDto model)
        {
            _logger.LogDebug("Put修改用户");
            if (ModelState.IsValid)
            {
                var user = await _userManager.Value.FindByIdAsync(model.Id);
                if (user == null)
                {
                    _logger.LogError($"用户修改错误：{model.UserName} 不存在");
                    return ResultModel.Failed($"用户修改错误：{model.UserName} 不存在");
                }

                var verification = await UpdateUserBeforeVerification(rank, user, model);
                if (!verification.Success)
                {
                    return verification;
                }

                //user = _mapper.Value.Map<IdentityUser>(model);
                //var result = await _userManager.Value.UpdateAsync(user);//不能整个model更新！！！
                //目前只需要对 IdentityUser 的用户名、邮箱，手机号进行更新操作
                if (user.UserName != model.UserName || user.Email != model.Email)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    var resultUpdateAsync = await _userManager.Value.UpdateAsync(user);//Called to update the user after validating and updating the normalized email/user name.
                    if (!resultUpdateAsync.Succeeded)
                    {
                        _logger.LogError($"用户修改错误：{resultUpdateAsync.Errors.First().Description}");
                        return ResultModel.Failed($"用户修改错误：{resultUpdateAsync.Errors.First().Description}", (int)HttpStatusCode.InternalServerError);
                    }
                }
                if (user.PhoneNumber != model.PhoneNumber)
                {
                    var changePhoneNumberToken = await _userManager.Value.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
                    var resultUpdateAsync = await _userManager.Value.ChangePhoneNumberAsync(user, model.PhoneNumber, changePhoneNumberToken);
                    if (!resultUpdateAsync.Succeeded)
                    {
                        _logger.LogError($"用户修改错误：{resultUpdateAsync.Errors.First().Description}");
                        return ResultModel.Failed($"用户修改错误：{resultUpdateAsync.Errors.First().Description}", (int)HttpStatusCode.InternalServerError);
                    }
                }

                var resultUpdateUserClaimExtras = await UpdateUserClaimExtras(user, model);
                if (!resultUpdateUserClaimExtras.Succeeded)
                {
                    _logger.LogError($"用户修改错误：{resultUpdateUserClaimExtras.Errors.First().Description}");
                    return ResultModel.Failed($"用户修改错误：{resultUpdateUserClaimExtras.Errors.First().Description}", (int)HttpStatusCode.InternalServerError);
                }

                var data = await _userManager.Value.FindByNameAsync(model.UserName);
                var userDto = _mapper.Value.Map<UserDto>(data);
                await UserDtoSetClaimExtras(userDto);
                return ResultModel.Success(userDto);
            }
            return ResultModel.Failed(ModelStateErrorMessage(ModelState), (int)HttpStatusCode.BadRequest);
        }

        [Description("根据请求者职位等级Patch修改用户，成功返回用户信息")]
        [OperationId("根据请求者职位等级Patch修改用户")]
        [Parameters(name = "rank", param = "请求者职位等级")]
        [Parameters(name = "name", param = "用户名")]
        [HttpPatch]
        public async Task<IResultModel> PatchUpdateByName([Required] EnumRole rank, string name, [FromBody] JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            _logger.LogDebug("Patch修改用户");
            if (ModelState.IsValid)
            {
                var user = await _userManager.Value.FindByNameAsync(name);
                if (user == null)
                {
                    _logger.LogError($"用户修改错误：{name} 不存在");
                    return ResultModel.Failed($"用户修改错误：{name} 不存在");
                }
                var modelRouteToPatch = _mapper.Value.Map<UserUpdateDto>(user);
                await UserUpdateDtoSetClaimExtras(modelRouteToPatch);
                patchDocument.ApplyTo(modelRouteToPatch);

                var verification = await UpdateUserBeforeVerification(rank, user, modelRouteToPatch);
                if (!verification.Success)
                {
                    return verification;
                }

                //_mapper.Value.Map(modelRouteToPatch, user);
                //var result = await _userManager.Value.UpdateAsync(user);//不能整个model更新！！！
                //if (!result.Succeeded)
                //{
                //    _logger.LogError($"用户修改错误：{result.Errors.First().Description}");
                //    return ResultModel.Failed($"用户修改错误：{result.Errors.First().Description}");
                //}
                //目前只需要对 IdentityUser 的用户名、邮箱，手机号进行更新操作
                if (user.UserName != modelRouteToPatch.UserName || user.Email != modelRouteToPatch.Email)
                {
                    user.UserName = modelRouteToPatch.UserName;
                    user.Email = modelRouteToPatch.Email;
                    var resultUpdateAsync = await _userManager.Value.UpdateAsync(user);//Called to update the user after validating and updating the normalized email/user name.
                    if (!resultUpdateAsync.Succeeded)
                    {
                        _logger.LogError($"用户修改错误：{resultUpdateAsync.Errors.First().Description}");
                        return ResultModel.Failed($"用户修改错误：{resultUpdateAsync.Errors.First().Description}", (int)HttpStatusCode.InternalServerError);
                    }
                }
                if (user.PhoneNumber != modelRouteToPatch.PhoneNumber)
                {
                    var changePhoneNumberToken = await _userManager.Value.GenerateChangePhoneNumberTokenAsync(user, modelRouteToPatch.PhoneNumber);
                    var resultUpdateAsync = await _userManager.Value.ChangePhoneNumberAsync(user, modelRouteToPatch.PhoneNumber, changePhoneNumberToken);
                    if (!resultUpdateAsync.Succeeded)
                    {
                        _logger.LogError($"用户修改错误：{resultUpdateAsync.Errors.First().Description}");
                        return ResultModel.Failed($"用户修改错误：{resultUpdateAsync.Errors.First().Description}", (int)HttpStatusCode.InternalServerError);
                    }
                }

                var result = await UpdateUserClaimExtras(user, modelRouteToPatch);
                if (!result.Succeeded)
                {
                    _logger.LogError($"用户修改错误：{result.Errors.First().Description}");
                    return ResultModel.Failed($"用户修改错误：{result.Errors.First().Description}");
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
            IdentityResult result = IdentityResult.Success;
            if (userDto.Rank != model.Rank)
            {
                result = await _userManager.Value.ReplaceClaimAsync(user, new Claim("rank", userDto.Rank.ToString()), new Claim("rank", model.Rank.ToString()));
                if (!result.Succeeded)
                {
                    return result;
                }
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
            if (userDto.IsFreeze != model.IsFreeze)
            {
                await _userManager.Value.ReplaceClaimAsync(user, new Claim("isfreeze", userDto.IsFreeze.ToString()), new Claim("isfreeze", model.IsFreeze.ToString()));
                if (!result.Succeeded)
                {
                    return result;
                }
            }
            return result;
        }

        private async Task<bool> CheckStoreManagerExistByUserClaims(IList<Claim> userClaims)
        {
            var dataStoreManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", EnumRole.StoreManager.ToString()));
            var dataByShopId = await _userManager.Value.GetUsersForClaimAsync(new Claim("shopid", userClaims.FirstOrDefault(c => c.Type == "shopid").Value));
            var dataByStoreId = await _userManager.Value.GetUsersForClaimAsync(new Claim("storeid", userClaims.FirstOrDefault(c => c.Type == "storeid").Value));
            var storeManagerList = dataByShopId.Intersect(dataByStoreId).Intersect(dataStoreManager).ToList();
            if (storeManagerList == null || storeManagerList.Count == 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> CheckStoreManagerExistByShopIdAndStoreId(string shopid, string storeid)
        {
            var dataStoreManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", EnumRole.StoreManager.ToString()));
            var dataByShopId = await _userManager.Value.GetUsersForClaimAsync(new Claim("shopid", shopid));
            var dataByStoreId = await _userManager.Value.GetUsersForClaimAsync(new Claim("storeid", storeid));
            var storeManagerList = dataByShopId.Intersect(dataByStoreId).Intersect(dataStoreManager).ToList();
            if (storeManagerList == null || storeManagerList.Count == 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> CheckShopManagerExistByShopId(string shopid)
        {
            var dataShopManager = await _userManager.Value.GetUsersForClaimAsync(new Claim("rank", EnumRole.ShopManager.ToString()));
            var dataByShopId = await _userManager.Value.GetUsersForClaimAsync(new Claim("shopid", shopid));
            var shopManagerList = dataByShopId.Intersect(dataShopManager).ToList();
            if (shopManagerList == null || shopManagerList.Count == 0)
            {
                return false;
            }
            return true;
        }

        private async Task<IList<IdentityUser>> IdentityUsersFilterByRank(IList<IdentityUser> data, EnumRole rank)
        {
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
            return data;
        }

        private async Task<IResultModel> UpdateUserBeforeVerification(EnumRole rank, IdentityUser user, UserUpdateDto model)
        {
            var userClaims = await _userManager.Value.GetClaimsAsync(user);
            var userRank = (EnumRole)Enum.Parse(typeof(EnumRole), userClaims.FirstOrDefault(c => c.Type == "rank").Value);
            var modelRank = model.Rank;
            if (rank >= userRank && userRank != EnumRole.ShopManager)
            {
                _logger.LogError($"用户修改错误：{rank.ToDescription()} 操作权限有限");
                return ResultModel.Failed($"用户修改错误：{rank.ToDescription()} 操作权限有限");
            }
            if (user.UserName != model.UserName)
            {
                var userExist = await _userManager.Value.FindByNameAsync(model.UserName);
                if (userExist != null)
                {
                    _logger.LogError($"用户修改错误：{model.UserName} 已被占用");
                    return ResultModel.Failed($"用户修改错误：{model.UserName} 已被占用");
                }
            }
            if (userRank == EnumRole.ShopManager && model.IsFreeze)
            {
                _logger.LogError($"用户修改错误：{user.UserName} 是老板职位不能被禁用");
                return ResultModel.Failed($"用户修改错误：{user.UserName} 是老板职位不能被禁用");
            }
            if (userRank == EnumRole.ShopManager && model.Rank != EnumRole.ShopManager)
            {
                _logger.LogError($"用户修改错误：{user.UserName} 是老板职位不能更改自己的职位");
                return ResultModel.Failed($"用户修改错误：{user.UserName} 是老板职位不能更改自己的职位");
            }
            if (userRank != EnumRole.ShopManager && model.Rank == EnumRole.ShopManager)
            {
                _logger.LogError($"用户修改错误：{user.UserName} 不能更改为老板职位");
                return ResultModel.Failed($"用户修改错误：{user.UserName} 不能更改为老板职位");
            }
            if (userRank != EnumRole.StoreManager && model.Rank == EnumRole.StoreManager)
            {
                var storeManagerExist = await CheckStoreManagerExistByUserClaims(userClaims);
                if (storeManagerExist)
                {
                    _logger.LogError($"用户修改错误：店长已经存在，{user.UserName} 不能更改为店长职位");
                    return ResultModel.Failed($"用户修改错误：店长已经存在，{user.UserName} 不能更改为店长职位");
                }
            }
            return ResultModel.Success();
        }

        private async Task<IResultModel> DeleteUserBeforeVerification(EnumRole rank, IdentityUser user)
        {
            var userClaims = await _userManager.Value.GetClaimsAsync(user);         
            if (rank >= (EnumRole)Enum.Parse(typeof(EnumRole), userClaims.FirstOrDefault(c => c.Type == "rank").Value))
            {
                _logger.LogError($"用户删除错误：{rank.ToDescription()} 操作权限有限");
                return ResultModel.Failed($"用户删除错误：{rank.ToDescription()} 操作权限有限");
            }
            return ResultModel.Success();
        }

        private async Task<IResultModel> AddUserBeforeVerification(EnumRole rank, UserCreateDto model)
        {
            if (rank >= model.Rank)
            {
                _logger.LogError($"用户新增错误：{rank.ToDescription()} 操作权限有限");
                return ResultModel.Failed($"用户新增错误：{rank.ToDescription()} 操作权限有限");
            }
            if (model.Rank == EnumRole.StoreManager)
            {
                var storeManagerExist = await CheckStoreManagerExistByShopIdAndStoreId(model.ShopId.ToString(), model.StoreId.ToString());
                if (storeManagerExist)
                {
                    _logger.LogError($"用户新增错误：店长已经存在，{model.UserName} 不能再新增为店长职位");
                    return ResultModel.Failed($"用户新增错误：店长已经存在，{model.UserName} 不能再新增为店长职位");
                }
            }
            if (model.Rank == EnumRole.ShopManager)
            {
                var shopManagerExist = await CheckShopManagerExistByShopId(model.ShopId.ToString());
                if (shopManagerExist)
                {
                    _logger.LogError($"用户新增错误：老板已经存在，{model.UserName} 不能再新增为老板职位");
                    return ResultModel.Failed($"用户新增错误：老板已经存在，{model.UserName} 不能再新增为老板职位");
                }
            }
            if (model.Rank == EnumRole.ShopAssistant)
            {
                if (model.ShopId != model.StoreId)//默认总部门店id=商店id
                {
                    //通过商店id 也是门店id 获取门店 门店名称
                    var result = await _storeService.Value.GetByStoreIdAsync(model.ShopId);
                    var storeName = ((IResultModel<StoreDto>)result)?.Data?.Name;
                    _logger.LogError($"用户新增错误：老板助理需要新增到 {storeName} 门店下");
                    return ResultModel.Failed($"用户新增错误：老板助理需要新增到 {storeName} 门店下");
                }
            }
            return ResultModel.Success();
        }
    }
}


//固定业务备注：

//一个商店只有一个老板（在注册时创建，删除添加要做约束处理），可以有多个老板助理（所属门店应为总部，即门店id=商店id，添加要做约束处理）；
//一个商店可以有多个门店（必须要有最多一个总部门店，即门店id=商店id，删除门店要做处理），一个门店最多只有一个店长（添加要做约束处理），可以有多个店长助理，可以有多个收银员
//老板和老板助理职位能查全部商店所有门店用户，其他职位用户查所在当前门店下该职位下（包含该职位）的所有用户。  
//职位的管理范围是该职位下的所有职位，如添加修改删除用户只能操作该职位以下（不包含该职位）的所有职位用户（同职位用户不能被添加修改删除，也就是说自己也不能修改删除自己，需要上级修改；老板可以自己修改，但是不能删除；所有用户密码开放为自己修改）
//在用户列表中按条件搜索用户时，是在该用户职位下可见的用户进行搜索，所以按职位搜索时，搜索的职位条件不可以大于该用户职位，否则将搜索不到数据（职位搜索约束需要前端控制）。

//后面使用消息推送，商店下的用户有更新时通知其他登录用户刷新或重启（如果更新的是已登录用户）