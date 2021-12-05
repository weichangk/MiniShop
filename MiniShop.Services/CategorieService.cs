using MiniShop.IServices;
using MiniShop.Model;
using MiniShop.Orm;

namespace MiniShop.Services
{
    public class CategorieService : BaseService<Categorie>, ICategorieService
    {
        public CategorieService(AppDbContext context)
        {
            _context = context;
        }
    }
}
