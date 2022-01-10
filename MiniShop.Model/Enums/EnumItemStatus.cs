using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 商品状态
    /// </summary>
    public enum EnumItemStatus
    {
        [Description("正常")]
        [Display(Name = "正常")]
        Normal,
        [Description("停售")]
        [Display(Name = "停售")]
        HaltSales,
        [Description("停购")]
        [Display(Name = "停购")]
        StopBuying,
        [Description("淘汰")]
        [Display(Name = "淘汰")]
        Obsolete,
    }
}
