using System;
using System.ComponentModel.DataAnnotations;

namespace MiniShopAdmin.Dto
{
    public class RenewPackageDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "续费包名称")]
        public string Name { get; set; }

        [Display(Name = "续费包价格")]
        public decimal Price { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreatedTime { get; set; }

        [Display(Name = "修改时间")]
        public DateTime ModifiedTime { get; set; }

        [Display(Name = "操作人")]
        public string OperatorName { get; set; }

        [Display(Name = "续费包图片")]
        public string Image { get; set; }
    }
}
