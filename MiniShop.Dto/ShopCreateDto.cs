using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class ShopCreateDto
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        [MaxLength(32)]
        public string Contacts { get; set; }
        [Required]
        [MaxLength(32)]
        [RegularExpression(@"^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\d{8}$")]
        public string Phone { get; set; }
        [MaxLength(32)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        [MaxLength(64)]
        public string Address { get; set; }
    }
}
