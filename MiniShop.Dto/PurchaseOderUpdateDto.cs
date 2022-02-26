using MiniShop.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class PurchaseOderUpdateDto
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int Id { get; set; }

        [Display(Name = "供应商ID")]
        [Required(ErrorMessage = "{0},不能为空")]
        public int SupplierId { get; set; }

        [Display(Name = "审核状态")]
        public EnumAuditStatus AuditState { get; set; } = EnumAuditStatus.UnAudited;

        [Display(Name = "订单状态")]
        public EnumPurchaseOrderStatus OrderState { get; set; } = EnumPurchaseOrderStatus.UnReceived;

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "采购订单商品列表信息")]
        [Required(ErrorMessage = "{0},不能为空")]
        public List<PurchaseOderItemUpdateDto> PurchaseOderItemUpdateDtos { get; set; }
    }
}
