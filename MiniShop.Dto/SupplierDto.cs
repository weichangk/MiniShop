using MiniShop.Model.Code;
using MiniShop.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class SupplierDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "商店ID")]
        public Guid ShopId { get; set; }

        [Display(Name = "供应商编码")]
        public int Code { get; set; }

        [Display(Name = "供应商名称")]
        public string Name { get; set; }

        [Display(Name = "联系人")]
        public string Contacts { get; set; }

        [Display(Name = "手机号")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "状态")]
        public EnumSupplierStatus State { get; set; }

        [Display(Name = "状态描述")]
        public string StateDes => State.ToDescription();

        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}
