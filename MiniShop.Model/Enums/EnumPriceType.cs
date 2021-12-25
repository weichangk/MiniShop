using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model
{
    /// <summary>
    /// 计价方式
    /// </summary>
    public enum EnumPriceType
    {
        [Description("普通")]
        [Display(Name = "普通")]
        General,
        [Description("计数")]
        [Display(Name = "计数")]
        Count,
        [Description("计重")]
        [Display(Name = "计重")]
        Weight,
    }
}
