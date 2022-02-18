using MiniShop.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class SupplierCreateDto
    {
        [Display(Name = "商店ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public Guid ShopId { get; set; }

        [Display(Name = "供应商编码")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Code { get; set; }

        [Display(Name = "供应商名称")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Name { get; set; }

        [Display(Name = "联系人")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Contacts { get; set; }

        [Display(Name = "手机号")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "状态")]
        public EnumSupplierStatus State { get; set; }
    }
}
