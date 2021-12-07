using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MiniShop.Mvc
{
    public class IdsDbContext : IdentityDbContext<IdentityUser>
    {
        public IdsDbContext(DbContextOptions<IdsDbContext> options) : base(options)
        {
        }
    }
}
