using System.ComponentModel.DataAnnotations;

namespace MiniShop.Dto
{
    public class PurchaseOderItemDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "商品ID")]
        public int ItemId { get; set; }

        [Display(Name = "货号")]
        public string ItemCode { get; set; }

        [Display(Name = "品名")]
        public string ItemName { get; set; }

        [Display(Name = "单位")]
        public string UnitName { get; set; }

        [Display(Name = "进货价")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "数量")]
        public decimal Number { get; set; }

        [Display(Name = "赠送数量")]
        public decimal GiveNumber { get; set; }

        [Display(Name = "小计金额")]
        public decimal Amount { get; set; }

        [Display(Name = "实际进货价")]
        public decimal RealPurchasePrice { get; set; }
    }
}
