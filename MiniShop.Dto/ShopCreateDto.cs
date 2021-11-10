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
        public string Phone { get; set; }
        [Required]
        [MaxLength(32)]
        public string Email { get; set; }
        [Required]
        [MaxLength(32)]
        public string Password { get; set; }
        [MaxLength(32)]
        public string ConfirmPassword { get; set; }
        [Required]
        [MaxLength(64)]
        public string Address { get; set; }
    }
}
