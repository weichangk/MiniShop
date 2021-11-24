using Microsoft.EntityFrameworkCore;
using MiniShop.Api.Database;
using MiniShop.Model;
using MiniShop.Model.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Api.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User CreateShopManagerUser(string userName, string phone, string email)
        {
            if (phone == null) phone = "";
            if (email == null) email = "";

            Guid shopId = Guid.NewGuid();
            User user = new User
            {
                ShopId = shopId,
                Name = userName,
                Phone = phone,
                Email = email,
                Role = EnumRole.ShopManager,
            };

            _context.Set<User>().Add(user);
            return user;
        }


        public Task<User> UserExist(string userName)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Name.Equals(userName));
            return user;
        }
    }
}
