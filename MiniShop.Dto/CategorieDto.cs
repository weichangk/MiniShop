using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class CategorieDto
    {
        [Required]
        public Guid ShopId { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
