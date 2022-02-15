using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniShop.Dto
{
    public class CategorieUpdateDto
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Id { get; set; }

        [Display(Name = "商店ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public Guid ShopId { get; set; }

        [Display(Name = "类别编码")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Code { get; set; }

        [Display(Name = "类别名称")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Name { get; set; }
    }
}
