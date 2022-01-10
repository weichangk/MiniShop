using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class ShopDto
    {
        [Display(Name = "商店ID")]
        public Guid Id { get; set; }

        [Display(Name = "商店名称")]
        public string Name { get; set; }

        [Display(Name = "联系人")]
        public string Contacts { get; set; }

        [Display(Name = "手机号")]
        public string Phone { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }
    }
}
