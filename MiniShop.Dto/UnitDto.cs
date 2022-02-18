using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UnitDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "商店ID")]
        public Guid ShopId { get; set; }

        [Display(Name = "单位编码")]
        public int Code { get; set; }

        [Display(Name = "单位名称")]
        public string Name { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}
