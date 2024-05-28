namespace WebAppStarter.Api.Security;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using WebAppStarter.Api.Models;

/// <summary>
/// An abstract authorization filter that validates user access based on configured scopes and application permissions.
/// </summary>
/// <remarks>
/// This class checks if the current user has at least one of the required scopes or application permissions specified in the application's configuration.
/// If the user does not have the required scopes or permissions, the request is forbidden.
/// </remarks>
public abstract class PermissionFilter(
    IConfiguration configuration,
    IOptions<ClaimSettings> claimSettings,
    string? requiredScopesConfigurationKey,
    string? requiredAppPermissionsConfigurationKey
) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (requiredScopesConfigurationKey == null)
        {
            context.Result = new ForbidResult(); // Use StatusCodeResult(403) if you prefer a simple 403 response
            return;
        }
        // Retrieve scopes and permissions from configuration
        var requiredScopes =
            requiredScopesConfigurationKey != null
                ? configuration.GetSection(requiredScopesConfigurationKey).Get<List<string>>()
                : null;
        var requiredPermissions =
            requiredAppPermissionsConfigurationKey != null
                ? configuration
                    .GetSection(requiredAppPermissionsConfigurationKey)
                    .Get<List<string>>()
                : null;

        // Check that the user claims has the correct scope and role
        var hasValidScope =
            requiredScopes?.Any(scope =>
                context.HttpContext.User.Claims.Any(c =>
                    c.Type == claimSettings.Value.ScopeClaimType && c.Value.Contains(scope) // <-- string.Contains??
                )
            ) ?? false; // Maybe early-out on requiredScopes == null?

        // TODO: replace claimSettings.Value.RoleClaimType with ClaimTypes....
        var hasValidPermission =
            requiredPermissions?.Any(permission =>
                context.HttpContext.User.Claims.Any(c =>
                    c.Type == claimSettings.Value.RoleClaimType && c.Value.Contains(permission)
                )
            ) ?? false;

        if (!hasValidScope || !hasValidPermission)
        {
            context.Result = new ForbidResult(); // Use StatusCodeResult(403) if you prefer a simple 403 response
        }
    }
}
