using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class ShopDto
    {
        [Required]
        public Guid Id { get; set; }
        [Display(Name = "商店名称")]
        [Required(ErrorMessage ="{0},不能为空")]
        [MaxLength(32)]
        public string Name { get; set; }
        [Display(Name = "联系人")]
        [Required(ErrorMessage = "{0},不能为空")]
        [MaxLength(32)]
        public string Contacts { get; set; }
        [Display(Name = "手机号")]
        [Required(ErrorMessage = "{0},不能为空")]
        [MaxLength(32)]
        [RegularExpression(@"^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\d{8}$")]
        public string Phone { get; set; }
        [Display(Name = "邮箱")]
        [MaxLength(32)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
        [Display(Name = "地址")]
        [MaxLength(64)]
        public string Address { get; set; }
    }
}
