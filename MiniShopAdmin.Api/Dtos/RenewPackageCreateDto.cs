using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShopAdmin.Api.Dtos
{
    public class RenewPackageCreateDto
    {
        [Display(Name = "续费包名称")]
        [Required(ErrorMessage = "{0},不能为空")]
        public string Name { get; set; }

        [Display(Name = "续费包价格")]
        [Required(ErrorMessage = "{0},不能为空")]
        public decimal Price { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [Display(Name = "操作人")]
        public string OperatorName { get; set; }
    }
}
