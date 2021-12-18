using MiniShop.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UserDto
    {
        [Display(Name = "用户ID")]
        public int Id { get; set; }

        [Display(Name = "商店ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public Guid ShopId { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0},不能为空")]
        [StringLength(32, ErrorMessage = "{0},不能大于{1}")]
        public string Name { get; set; }

        [Display(Name = "手机号")]
        [RegularExpression(@"^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\d{8}$", ErrorMessage = "{0}的格式不正确")]
        public string Phone { get; set; }

        [Display(Name = "邮箱")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "{0}的格式不正确")]
        public string Email { get; set; }

        [Display(Name = "角色")]
        [Required(ErrorMessage = "{0},不能为空")]
        public EnumRole Role { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        [StringLength(11, ErrorMessage = "{0},不能小于{2}，最长{1}", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("PassWord", ErrorMessage = "密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
}
