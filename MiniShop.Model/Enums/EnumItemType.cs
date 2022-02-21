using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 商品类型
    /// </summary>
    public enum EnumItemType
    {
        [Description("普通商品")]
        [Display(Name = "普通商品")]
        Normal,
    }
}
