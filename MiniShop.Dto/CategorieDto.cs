using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class CategorieDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "商店ID")]
        public Guid ShopId { get; set; }

        [Display(Name = "类别编码")]
        public int Code { get; set; }

        [Display(Name = "类别名称")]
        public string Name { get; set; }

        [Display(Name = "类别等级")]
        public int Level { get; set; }

        [Display(Name = "父类别编码")]
        public int ParentCode { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; }
    }
}
