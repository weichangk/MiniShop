using System.Collections.Generic;

namespace MiniShopAdmin.Api.Models.Code
{
    public class InitializationData
    {
        public List<RenewPackage> RenewPackage { get; set; }
        public static InitializationData Initialization { get; set; } = new InitializationData();
    }
}
