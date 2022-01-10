using Microsoft.AspNetCore.Http;
using MiniShop.Model.Enums;
using System;
using System.Globalization;

namespace MiniShop.Mvc.Code
{
    public class LoginInfo : ILoginInfo
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginInfo(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string LoginIdToken
        {
            get
            {
                var loginIdToken = _contextAccessor?.HttpContext?.User?.FindFirst("LoginIdToken");
                if (loginIdToken == null || string.IsNullOrWhiteSpace(loginIdToken.Value))
                {
                    throw new Exception("LoginIdToken Not Found");
                }
                return loginIdToken.Value;
            }
        }

        public string LoginAccessToken
        {
            get
            {
                var accessToken = _contextAccessor?.HttpContext?.User?.FindFirst("LoginAccessToken");
                if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.Value))
                {
                    throw new Exception("LoginAccessToken Not Found");
                }
                return accessToken.Value;
            }
        }

        public string LoginRefreshToken
        {
            get
            {
                var refreshToken = _contextAccessor?.HttpContext?.User?.FindFirst("LoginRefreshToken");
                if (refreshToken == null || string.IsNullOrWhiteSpace(refreshToken.Value))
                {
                    throw new Exception("LoginRefreshToken Not Found");
                }
                return refreshToken.Value;
            }
        }

        public DateTime LoginExpiresAt
        {
            get
            {
                var expiresAt = _contextAccessor?.HttpContext?.User?.FindFirst("LoginExpiresAt");
                if (expiresAt == null || string.IsNullOrWhiteSpace(expiresAt.Value))
                {
                    throw new Exception("LoginExpiresAt Not Found");
                }
                var dataExp = DateTime.Parse(expiresAt.Value, null, DateTimeStyles.RoundtripKind);
                return dataExp;
            }
        }

        public int LoginId
        {
            get
            {
                var loginId = _contextAccessor?.HttpContext?.User?.FindFirst("LoginId");
                if (loginId == null || string.IsNullOrWhiteSpace(loginId.Value))
                {
                    throw new Exception("LoginId Not Found");
                }
                return int.Parse(loginId.Value);
            }
        }

        public Guid LoginShopId
        {
            get
            {
                var loginShopId = _contextAccessor?.HttpContext?.User?.FindFirst("LoginShopId");
                if (loginShopId == null || string.IsNullOrWhiteSpace(loginShopId.Value))
                {
                    throw new Exception("LoginShopId Not Found");
                }
                return Guid.Parse(loginShopId.Value);
            }
        }

        public string LoginName
        {
            get
            {
                var loginName = _contextAccessor?.HttpContext?.User?.FindFirst("LoginName");
                if (loginName == null || string.IsNullOrWhiteSpace(loginName.Value))
                {
                    throw new Exception("LoginName Not Found");
                }
                return loginName.Value;
            }
        }

        public string LoginPhone
        {
            get
            {
                var loginPhone = _contextAccessor?.HttpContext?.User?.FindFirst("LoginPhone");
                if (loginPhone == null || string.IsNullOrWhiteSpace(loginPhone.Value))
                {
                    throw new Exception("LoginPhone Not Found");
                }
                return loginPhone.Value;
            }
        }

        public string LoginEmail
        {
            get
            {
                var loginEmail = _contextAccessor?.HttpContext?.User?.FindFirst("LoginEmail");
                if (loginEmail == null || string.IsNullOrWhiteSpace(loginEmail.Value))
                {
                    throw new Exception("LoginEmail Not Found");
                }
                return loginEmail.Value;
            }
        }

        public EnumRole LoginRole
        {
            get
            {
                var loginRole = _contextAccessor?.HttpContext?.User?.FindFirst("LoginRole");
                if (loginRole == null || string.IsNullOrWhiteSpace(loginRole.Value))
                {
                    throw new Exception("LoginRole Not Found");
                }
                return (EnumRole)Enum.Parse(typeof(EnumRole), loginRole.Value) ;
            }
        }
    }
}
