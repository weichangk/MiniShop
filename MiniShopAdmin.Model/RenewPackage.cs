namespace MiniShopAdmin.Model
{
    public class RenewPackage : EntityBaseNoDeleted<int>
    {
        public decimal Price { get; set; }

        public int Months { get; set; }

        public string Image { get; set; }

        public string Remark { get; set; }
    }
}
