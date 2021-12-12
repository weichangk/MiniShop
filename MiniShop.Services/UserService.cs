using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniShop.IServices;
using MiniShop.Model;
using MiniShop.Model.Enums;
using MiniShop.Orm;
using System;
using System.Threading.Tasks;

namespace MiniShop.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(AppDbContext context, ILogger<UserService> logger) : base(logger)
        {
            _context = context;
        }

        public User CreateDefaultShopAndUser(string userName, string phone, string email)
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
            };
            Shop shop = new Shop
            {
                Id = shopId,
                Name = $"{userName} Shop",
                Contacts = userName,
                Phone = phone,
                Email = email,
                CreateDate = dateTime,
                ValidDate = dateTime.AddDays(7),
            };
            _context.Set<User>().Add(user);
            _context.Set<Shop>().Add(shop);
            return user;
        }

        public Task<User> UserExist(string userName)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Name.Equals(userName));
            return user;
        }
    }
}
