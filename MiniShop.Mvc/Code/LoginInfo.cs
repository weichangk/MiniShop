using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Mvc.Code
{
    public class LoginInfo : ILoginInfo
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginInfo(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string AccessToken
        {
            get
            {

                var accessToken = _contextAccessor?.HttpContext?.User?.FindFirst("AccessToken");

                if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.Value))
                {
                    return "";
                }
                return accessToken.Value;
            }
        }

        public string RefreshToken
        {
            get
            {
                var refreshToken = _contextAccessor?.HttpContext?.User?.FindFirst("RefreshToken");

                if (refreshToken == null || string.IsNullOrWhiteSpace(refreshToken.Value))
                {
                    return "";
                }
                return refreshToken.Value;
            }
        }

        public string ExpiresIn
        {
            get
            {
                var expiresIn = _contextAccessor?.HttpContext?.User?.FindFirst("ExpiresIn");

                if (expiresIn == null || string.IsNullOrWhiteSpace(expiresIn.Value))
                {
                    return "";
                }
                return expiresIn.Value;
            }
        }
    }
}
