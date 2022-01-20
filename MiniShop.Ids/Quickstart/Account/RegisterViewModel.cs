using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class RegisterViewModel
    {
        [Display(Name = "用户名")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{4,16}$", ErrorMessage = "{0}的格式不正确，4到16位（字母，数字，下划线，减号）")]
        public string Username { get; set; }

        [Display(Name = "邮箱")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Display(Name = "手机号")]
        [RegularExpression(@"^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\d{8}$", ErrorMessage = "{0}的格式不正确")]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        [RegularExpression(@"^.*(?=.{6,})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$", ErrorMessage = "{0}的格式不正确，最少6位，包括至少1个大写字母，1个小写字母，1个数字，1个特殊字符")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配")]
        public string ConfirmPassword { get; set; }

        public bool RegisterSucceeded { get; set; }

        public string ReturnUrl { get; set; }
    }
}