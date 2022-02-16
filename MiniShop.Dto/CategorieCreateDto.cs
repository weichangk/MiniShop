using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniShop.Dto
{
    public class CategorieCreateDto
    {
        [Display(Name = "商店ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public Guid ShopId { get; set; }

        [Display(Name = "类别编码")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Code { get; set; }

        [Display(Name = "类别名称")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Name { get; set; }

        [Display(Name = "类别等级")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Level { get; set; }

        [Display(Name = "父类别编码")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int ParentCode { get; set; }
    }
}
