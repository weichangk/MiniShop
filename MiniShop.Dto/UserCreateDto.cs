using MiniShop.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UserCreateDto
    {
        [Required]
        public Guid ShopId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        [MaxLength(32)]
        public string Phone { get; set; }
        [MaxLength(32)]
        public string Email { get; set; }
        public EnumRole Role { get; set; }
        [MaxLength(32)]
        public string Password { get; set; }
        [MaxLength(32)]
        public string ConfirmPassword { get; set; }
    }
}
