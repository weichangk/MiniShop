using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.Dto;
using MiniShop.IServices;
using MiniShop.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using yrjw.ORM.Chimp;
using yrjw.ORM.Chimp.Result;

namespace MiniShop.Services
{
    public class UserService : BaseService<User, UserDto, int>, IUserService, IDependency
    {
        private readonly Lazy<IRepository<Shop>> _shopRepository;

        public UserService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<UserService> logger,
            Lazy<IRepository<User>> repository, Lazy<IRepository<Shop>> shopRepository) : base(mapper, unitOfWork, logger, repository)
        {
            _shopRepository = shopRepository;
        }

        public async Task<IResultModel> GetLoginInfoOrShopManagerFirstRegister(string userName, string role, string phone, string email)
        {
            UserDto userDto;
            var data = _repository.Value.TableNoTracking;
            var entity = await _repository.Value.TableNoTracking.FirstOrDefaultAsync(p => p.Name.ToUpper() == userName.ToUpper());
            if (entity == null)
            {
                if (role != null && role.Equals(EnumRole.ShopManager.ToString()))
                {
                    if (phone == null) phone = "";
                    if (email == null) email = "";

                    Guid shopId = Guid.NewGuid();
                    DateTime dateTime = DateTime.Now;
                    User user = new User
                    {
                        ShopId = shopId,
                        Name = userName,
                        Phone = phone,
                        Email = email,
                        Role = EnumRole.ShopManager,
                        CreatedTime = dateTime,
                    };
                    Shop shop = new Shop
                    {
                        Id = shopId,
                        Name = $"{userName} shop",
                        Contacts = userName,
                        Phone = phone,
                        Email = email,
                        CreatedTime = dateTime,
                        ValidDate = dateTime.AddDays(7),
                    };

                    if (userName == "mini")
                    {
                        shop.ValidDate = dateTime.AddYears(99);
                    }

                    await _repository.Value.InsertAsync(user);
                    await _shopRepository.Value.InsertAsync(shop);
                    if (await UnitOfWork.SaveChangesAsync() > 0)
                    {
                        userDto = _mapper.Value.Map<UserDto>(user);
                        return ResultModel.Success(userDto);
                    }

                    _logger.LogError($"error：CreateDefaultShopAndUser Insert Save failed");
                    return ResultModel.Failed("error：CreateDefaultShopAndUser Insert Save failed", 500);
                }
                else
                {
                    return ResultModel.Failed("首次登录注册默认商店和用户信息异常，必须为店长角色");
                }
            }
            userDto = _mapper.Value.Map<UserDto>(entity);
            return ResultModel.Success(userDto);
        }

        public async Task<IResultModel> GetUsersByShopId(Guid shopId)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.ShopId == shopId);
            var list = await data.ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToListAsync();
            return ResultModel.Success(list);
        }

        public async Task<IResultModel> GetPageUsersByShopId(int pageIndex, int pageSize, Guid shopId)
        {
            var data = _repository.Value.TableNoTracking;
            data = data.Where(s => s.ShopId == shopId);
            var list = await data.ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).ToPagedListAsync(pageIndex, pageSize);
            return ResultModel.Success(list);
        }

        public async Task<IResultModel> GetByNameAsync(string name)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.Name.ToUpper() == name.ToUpper());
            var userDto = await data.ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).FirstOrDefaultAsync();
            return ResultModel.Success(userDto);
        }

        public async Task<IResultModel> GetByPhoneAsync(string phone)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.Phone == phone);
            var userDto = await data.ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).FirstOrDefaultAsync();
            return ResultModel.Success(userDto);
        }

        public async Task<IResultModel> GetByEmailAsync(string email)
        {
            var data = _repository.Value.TableNoTracking.Where(s => s.Email.ToUpper() == email.ToUpper());
            var userDto = await data.ProjectTo<UserDto>(_mapper.Value.ConfigurationProvider).FirstOrDefaultAsync();
            return ResultModel.Success(userDto);
        }
    }


    public class CreateUserService : BaseService<User, UserCreateDto, int>, ICreateUserService, IDependency
    {
        public CreateUserService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<CreateUserService> logger,
            Lazy<IRepository<User>> repository) : base(mapper, unitOfWork, logger, repository)
        {

        }
    }


    public class UpdateUserService : BaseService<User, UserUpdateDto, int>, IUpdateUserService, IDependency
    {
        public UpdateUserService(Lazy<IMapper> mapper, IUnitOfWork unitOfWork, ILogger<UpdateUserService> logger, Lazy<IRepository<User>> repository)
        : base(mapper, unitOfWork, logger, repository)
        {
            
        }
    }
}
