using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum EnumSex
    {
        [Description("男")]
        [Display(Name = "男")]
        Male,
        [Description("女")]
        [Display(Name = "女")]
        Female,
    }
}
