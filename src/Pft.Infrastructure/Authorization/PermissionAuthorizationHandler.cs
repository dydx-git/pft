using Pft.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Pft.Infrastructure.Authorization;
internal sealed class PermissionAuthorizationHandler(IServiceProvider serviceProvider)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!context.User.Identity.IsAuthenticated)
            return;

        using var scope = serviceProvider.CreateScope();

        var authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

        var identityId = context.User.GetIdentityId();

        //TODO: Introduce caching to avoid calling the database every time
        HashSet<string> permissions = await authorizationService.GetPermissionsForUserAsync(identityId);

        if (permissions.Contains(requirement.Permissions))
            context.Succeed(requirement);


    }
}
