using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Api.Ids
{
    public class IdsUser : IdentityUser
    {
        [Required]
        public Guid ShopId { get; set; }

        [Required]
        public Guid StoreId { get; set; }

        public bool IsFreeze { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
