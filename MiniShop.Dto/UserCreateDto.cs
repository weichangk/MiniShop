using MiniShop.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UserCreateDto
    {
        [Display(Name = "商店ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public Guid ShopId { get; set; }

        [Display(Name = "用户名")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{4,16}$", ErrorMessage = "{0}的格式不正确，4到16位（字母，数字，下划线，减号）")]
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
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$", ErrorMessage = "{0}的格式不正确，最少6位，包括至少1个大写字母，1个小写字母，1个数字，1个特殊字符")]
        public string PassWord { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("PassWord", ErrorMessage = "密码和确认密码不匹配")]
        public string ConfirmPassWord { get; set; }
    }
}
