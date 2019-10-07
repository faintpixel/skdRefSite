using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SkdAPI
{
    public class HasRoleHandler : AuthorizationHandler<HasRoleRequirement>
    {
        private readonly string @ROLE_CLAIM = @"https://reference.sketchdaily.net/roles";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasRoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ROLE_CLAIM && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var roles = context.User.FindFirst(c => c.Type == ROLE_CLAIM && c.Issuer == requirement.Issuer).Value;

            if (roles == "admin")
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
