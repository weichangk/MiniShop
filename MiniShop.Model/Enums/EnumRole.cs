using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 角色
    /// </summary>
    public enum EnumRole
    {
        [Description("老板")]
        [Display(Name = "老板")]
        ShopManager,
        [Description("老板助理")]
        [Display(Name = "老板助理")]
        ShopAssistant,

        [Description("店长")]
        [Display(Name = "店长")]
        StoreManager,
        [Description("店长助理")]
        [Display(Name = "店长助理")]
        StoreAssistant,

        [Description("收银员")]
        [Display(Name = "收银员")]
        Cashier,
    }
}
