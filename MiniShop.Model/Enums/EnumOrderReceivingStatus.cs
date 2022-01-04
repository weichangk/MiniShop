using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Model.Enums
{
    /// <summary>
    /// 采购订单收货状态
    /// </summary>
    public enum EnumOrderReceivingStatus
    {
        [Description("已收货")]
        [Display(Name = "已收货")]
        Received,
        [Description("未收货")]
        [Display(Name = "未收货")]
        UnReceived,
        [Description("已退货")]
        [Display(Name = "已退货")]
        Returned,
        [Description("未退货")]
        [Display(Name = "未退货")]
        UnReturned,
        [Description("部分退货")]
        [Display(Name ="部分退货")]
        PartReturned,
    }
}
