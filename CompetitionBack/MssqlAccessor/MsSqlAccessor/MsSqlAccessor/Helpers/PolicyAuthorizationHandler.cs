using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;
using System.Security.Claims;

namespace MsSqlAccessor.Helpers
{
    public class AuthorizationRequirementPolicy : IAuthorizationRequirement {
        public string Role { get; }
        public AuthorizationRequirementPolicy(string role)
        {
            Role = role;
        }
    }
    public class PolicyAuthorizationHandler : AuthorizationHandler<AuthorizationRequirementPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirementPolicy requirement)
        {
            var hasApiAdminClaim = context.User.HasClaim("permissions", "pg:tenant:admin"); 
            var hasAdminClaim = context.User.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "admin");
            var hasManagerClaim = context.User.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "manager");
            var hasParticipantClaim = context.User.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "participant");



            if (hasApiAdminClaim){
                var claims = new List<Claim>
            {
                new Claim("http://zionet-api/user/claims/email", "system@system.system")
            };

                var appIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var additionalClaim = new Claim("http://zionet-api/user/claims/email", "system@system.system");
                appIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                context.User.AddIdentity(appIdentity);
            }

            if (requirement.Role == "admin" ) {
                if (hasAdminClaim || hasApiAdminClaim)
                {
                    context.Succeed(requirement);
                }
                else {
                    context.Fail();
                }
            }

            if (requirement.Role == "manager")
            {
                if (hasAdminClaim || hasManagerClaim || hasApiAdminClaim)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }

            if (requirement.Role == "participant")
            {
                if (hasAdminClaim || hasManagerClaim || hasParticipantClaim || hasApiAdminClaim)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }

            return Task.CompletedTask;
        }
    }
}
