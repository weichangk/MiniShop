using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 角色
    /// </summary>
    public enum EnumRole
    {
        [Description("店长")]
        [Display(Name = "店长")]
        ShopManager,
        [Description("管理员")]
        [Display(Name = "管理员")]
        Admin,
        [Description("收银员")]
        [Display(Name = "收银员")]
        Cashier,
    }
}
