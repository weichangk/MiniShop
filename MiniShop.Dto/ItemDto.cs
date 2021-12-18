using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class ItemDto
    {
        [Required]
        public Guid ShopId { get; set; }

        [Required]
        public int CategorieId { get; set; }

        [Required]
        [MaxLength(32)]
        public string Code { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
