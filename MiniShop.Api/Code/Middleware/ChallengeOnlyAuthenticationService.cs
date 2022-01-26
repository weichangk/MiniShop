using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniShop.Api.Code.Middleware
{
    public class ChallengeOnlyAuthenticationService : IAuthenticationService
    {
        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            throw new System.NotImplementedException();
        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new System.NotImplementedException();
        }
    }

    public class sdsd : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            throw new System.NotImplementedException();
        }
    }
}
