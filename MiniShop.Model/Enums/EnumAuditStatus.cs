using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 审核状态
    /// </summary>
    public enum EnumAuditStatus
    {
        [Description("未审核")]
        [Display(Name = "未审核")]
        UnAudited,
        [Description("已审核")]
        [Display(Name = "已审核")]
        Audited,
    }
}
