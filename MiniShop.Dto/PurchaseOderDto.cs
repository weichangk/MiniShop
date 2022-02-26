using MiniShop.Model.Code;
using MiniShop.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class PurchaseOderDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "单号")]
        public string OderNo { get; set; }

        [Display(Name = "门店")]
        public string StoreName { get; set; }

        [Display(Name = "供应商")]
        public string SupplierName { get; set; }

        [Display(Name = "单据金额")]
        public decimal OderAmount { get; set; }

        [Display(Name = "制单人员")]
        public string OperatorName { get; set; }

        [Display(Name = "制单日期")]
        public DateTime CreatedTime { get; set; }

        [Display(Name = "审核人员")]
        public string AuditOperatorName { get; set; }

        [Display(Name = "审核日期")]
        public DateTime? AuditTime { get; set; }

        [Display(Name = "审核状态")]
        public EnumAuditStatus AuditState { get; set; }

        [Display(Name = "审核状态描述")]
        public string AuditStateDes => AuditState.ToDescription();

        [Display(Name = "订单状态")]
        public EnumPurchaseOrderStatus OrderState { get; set; }

        [Display(Name = "订单状态描述")]
        public string OrderStateDes => OrderState.ToDescription();

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "采购订单商品列表信息")]
        public List<PurchaseOderItemDto> PurchaseOderItemDtos { get; set; } = new List<PurchaseOderItemDto>();
    }
}
