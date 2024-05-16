namespace WebAppStarter.Api.Permissions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebAppStarter.Api.Models;

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

/// <summary>
/// Check that the connected user has the right to access the todo api in read mode.
/// </summary>
public class WriteTodoPermissionFilter : PermissionFilter
{
    private const string RequiredScopesConfigurationKey = "AzureAD:Scopes:Write";
    private const string RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write";

    public WriteTodoPermissionFilter(IConfiguration configuration, IOptions<ClaimSettings> claimSettings)
        : base(configuration, claimSettings, RequiredScopesConfigurationKey, RequiredAppPermissionsConfigurationKey)
    {
    }
}

public class WriteTodoPermissionAttribute : TypeFilterAttribute<WriteTodoPermissionFilter>
{
}

#pragma warning restore SA1649 // File name should match first type name
#pragma warning restore SA1402 // File may only contain a single type
