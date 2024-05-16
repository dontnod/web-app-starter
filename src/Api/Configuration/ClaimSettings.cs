namespace WebAppStarter.Api.Models;

public class ClaimSettings
{
    public required string ScopeClaimType { get; set; }

    public required string RoleClaimType { get; set; }
}