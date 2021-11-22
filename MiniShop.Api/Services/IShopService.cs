using MiniShop.Model;

namespace MiniShop.Api.Services
{
    public interface IShopService : IBaseService<Shop>
    {
        void CreateShopDefaultInfo(Shop shop);
    }
}
