using MiniShop.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 采购订单
    /// </summary>
    public class PurchaseReturnOder : EntityBaseNoDeletedStoreId<int>
    {
        [NotMapped]
        public override string Name { get => base.Name; set => base.Name = value; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OderNo { get; set; }

        /// <summary>
        /// 门店ID
        /// </summary>
        public new int StoreId { get; set; }

        /// <summary>
        /// 门店
        /// </summary>
        public virtual Store Store { get; set; } = new Store();

        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual Supplier Supplier { get; set; } = new Supplier();

        /// <summary>
        /// 采购退货订单商品列表信息
        /// </summary>
        public virtual ICollection<PurchaseReturnOderItem> PurchaseReturnOderItem { get; set; } = new List<PurchaseReturnOderItem>();

        /// <summary>
        /// 单据金额
        /// </summary>
        public decimal OderAmount { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public EnumAuditStatus AuditState { get; set; }

        /// <summary>
        /// 审核人员
        /// </summary>
        public string AuditOperatorName { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? AuditTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
