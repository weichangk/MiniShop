namespace MiniShopAdmin.Api.Models
{
    public class RenewPackage : EntityBaseNoDeleted<int>
    {
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
    }
}
