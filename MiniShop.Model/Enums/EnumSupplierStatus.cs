using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 供应商状态
    /// </summary>
    public enum EnumSupplierStatus
    {
        [Description("正常")]
        [Display(Name = "正常")]
        Normal,
        [Description("冻结")]
        [Display(Name = "冻结")]
        Freeze,
    }
}
