using MiniShop.Api.Database;
using MiniShop.Model;

namespace MiniShop.Api.Services
{
    public class CategorieService : BaseService<Categorie>, ICategorieService
    {
        public CategorieService(AppDbContext context)
        {
            _context = context;
        }
    }
}
