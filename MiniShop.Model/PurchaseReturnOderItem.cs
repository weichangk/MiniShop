using System.ComponentModel.DataAnnotations.Schema;

namespace MiniShop.Model
{
    /// <summary>
    /// 采购订单商品信息
    /// </summary>
    public class PurchaseReturnOderItem : EntityBaseNoDeleted<int>
    {
        [NotMapped]
        public override string Name { get => base.Name; set => base.Name = value; }

        /// <summary>
        /// 采购退货订单ID
        /// </summary>
        public int PurchaseReturnOderId { get; set; }

        /// <summary>
        /// 采购退货订单
        /// </summary>
        public virtual PurchaseReturnOder PurchaseReturnOder { get; set; } = new PurchaseReturnOder();

        /// <summary>
        /// 商品ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public virtual Item Item { get; set; } = new Item();

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// 赠送数量
        /// </summary>
        public decimal GiveNumber { get; set; }

        /// <summary>
        /// 小计金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 实际进货价
        /// </summary>
        public decimal PurchasePrice { get; set; }

    }
}
