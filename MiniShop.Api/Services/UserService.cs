using MiniShop.Api.Database;
using MiniShop.Model;

namespace MiniShop.Api.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(AppDbContext context)
        {
            _context = context;
        }
    }
}
