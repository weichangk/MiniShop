using MiniShop.Model;
using System;

namespace MiniShop.Mvc.Code
{
    public interface ILoginInfo
    {
        string LoginIdToken { get; }
        string LoginAccessToken { get; }
        string LoginRefreshToken { get; }
        DateTime LoginExpiresAt { get; }
        public int LoginId { get; }
        public Guid LoginShopId { get; }
        public string LoginName { get; }
        public string LoginPhone { get; }
        public string LoginEmail { get; }
        public EnumRole LoginRole { get; }
    }
}
