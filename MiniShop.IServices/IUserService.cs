using MiniShop.Model;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> UserExist(string userName);
        User CreateDefaultShopAndUser(string userName, string phone, string email);
    }
}
