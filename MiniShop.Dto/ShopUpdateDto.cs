using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class ShopUpdateDto
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Id { get; set; }

        [Display(Name = "商店名称")]
        [Required(ErrorMessage ="{0},不能为空")]
        [StringLength(32, ErrorMessage = "{0},不能大于{1}")]
        public string Name { get; set; }

        [Display(Name = "联系人")]
        [Required(ErrorMessage = "{0},不能为空")]
        [StringLength(32, ErrorMessage = "{0},不能大于{1}")]
        public string Contacts { get; set; }

        [Display(Name = "手机号")]
        [RegularExpression(@"^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\d{8}$", ErrorMessage = "{0}的格式不正确")]
        public string Phone { get; set; }

        [Display(Name = "邮箱")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "{0}的格式不正确")]
        public string Email { get; set; }

        [Display(Name = "地址")]
        [StringLength(64, ErrorMessage = "{0},不能大于{1}")]
        public string Address { get; set; }

        [Display(Name = "有效期")]
        [Required(ErrorMessage = "{0},不能为空")]
        public DateTime ValidDate { get; set; }
    }
}
