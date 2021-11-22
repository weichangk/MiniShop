using MiniShop.Model;
using System.Threading.Tasks;

namespace MiniShop.Api.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> UserExist(string userName);
        User CreateShopManagerUser(string userName, string phone, string email);
    }
}
