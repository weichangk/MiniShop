namespace MiniShop.Ids.Config
{
    public class BasicSetting
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 自宿主启动IP端口
        /// </summary>
        public string Urls { get; set; } = "http://*:5002";
        /// <summary>
        /// 主程序集名称
        /// </summary>
        public string AssemblyName { get; set; }


        public string MiniShopWeb_ApplicationUrl { get; set; }
        public string MiniShopWeb_ClientId { get; set; }
        public string MiniShopWeb_ClientName { get; set; }
        public string MiniShopWeb_ClientSecret { get; set; }
        public string MiniShopWeb_AccessTokenLifetime { get; set; }
        public string MiniShopWeb_AllowedScopes { get; set; }

        public string MiniShopAdminApiResource_Name { get; set; }
        public string MiniShopAdminApiResource_DisplayName { get; set; }
        public string MiniShopAdminApiResource_Secret { get; set; }
        public string MiniShopAdminApiResource_Scopes { get; set; }

        public string MiniShopApiResource_Name { get; set; }
        public string MiniShopApiResource_DisplayName { get; set; }
        public string MiniShopApiResource_Secret { get; set; }
        public string MiniShopApiResource_Scopes { get; set; }
        public string MiniShopApiResource_UserClaims { get; set; }

        public string UserClaimExtras_Name { get; set; }
        public string UserClaimExtras_DisplayName { get; set; }
        public string UserClaimExtras_UserClaims { get; set; }


        public static BasicSetting Setting { get; set; } = new BasicSetting();
    }
}