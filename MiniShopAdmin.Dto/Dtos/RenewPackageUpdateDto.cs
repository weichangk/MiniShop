using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShopAdmin.Dto
{
    public class RenewPackageUpdateDto
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Id { get; set; }

        [Display(Name = "续费包名称")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Name { get; set; }

        [Display(Name = "续费包价格")]
        [Required(ErrorMessage = "{0},不能为空")]
        public decimal Price { get; set; }

        [Display(Name = "修改时间")]
        public DateTime ModifiedTime { get; set; } = DateTime.Now;

        [Display(Name = "操作人")]
        public string OperatorName { get; set; }

        [Display(Name = "续费包图片")]
        public string Image { get; set; }
    }
}
