using System.Collections.Generic;

namespace MiniShopAdmin.Model.Code
{
    public class InitializationData
    {
        public List<RenewPackage> RenewPackage { get; set; }
        public static InitializationData Initialization { get; set; } = new InitializationData();
    }
}
