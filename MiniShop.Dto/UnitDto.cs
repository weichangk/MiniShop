using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UnitDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "单位ID")]
        public Guid ShopId { get; set; }

        [Display(Name = "单位编码")]
        public int Code { get; set; }

        [Display(Name = "单位名称")]
        public string Name { get; set; }
    }
}
