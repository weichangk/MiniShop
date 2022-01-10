using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 会员类型
    /// </summary>
    public enum EnumVipType
    {
        [Description("普通会员")]
        [Display(Name = "普通会员")]
        GeneralVip,
        [Description("黄金会员")]
        [Display(Name = "黄金会员")]
        GoldVip,
        [Description("白金会员")]
        [Display(Name = "白金会员")]
        PlatinaVip,
    }
}
