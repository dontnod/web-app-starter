namespace TodoApi.Models;

public class ClaimSettings
{
    public required string ScopeClaimType { get; set; }

    public required string RoleClaimType { get; set; }
}