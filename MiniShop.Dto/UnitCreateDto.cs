using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class UnitCreateDto
    {
        [Display(Name = "商店ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public Guid ShopId { get; set; }

        [Display(Name = "单位编码")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Code { get; set; }

        [Display(Name = "单位名称")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Name { get; set; }
    }
}
