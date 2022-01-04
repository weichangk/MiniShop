using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 采购订单状态
    /// </summary>
    public enum EnumPurchaseOrderStatus
    {      
        [Description("已审核")]
        [Display(Name = "已审核")]
        Audited,
        [Description("未审核")]
        [Display(Name = "未审核")]
        UnAudited,
    }
}
