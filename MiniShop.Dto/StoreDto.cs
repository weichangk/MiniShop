using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class StoreDto
    {
        [Display(Name = "门店ID")]
        public int Id { get; set; }

        [Display(Name = "商店ID")]
        public Guid ShopId { get; set; }

        [Display(Name = "门店名称")]
        public string Name { get; set; }

        [Display(Name = "联系人")]
        public string Contacts { get; set; }

        [Display(Name = "手机号")]
        public string Phone { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }
        
        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}
