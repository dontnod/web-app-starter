using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public static class MockHttpContext
{
    public static HttpContext CreateUserContext(Guid userId)
    {
        var context = new DefaultHttpContext();
        var claims = new List<Claim>
        {
            new("oid", userId.ToString()),
            new("scp", "user_access"), // Simple simulation; adjust as per actual claim setup
            new("roles", string.Empty),
        };
        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
        return context;
    }

    public static HttpContext CreateAppContext(Guid appId)
    {
        var context = new DefaultHttpContext();
        var claims = new List<Claim>
        {
            new("oid", appId.ToString()),
            new("scp",  string.Empty),
            new("idtyp",  "app"),
            new("roles", "application_role"),
        };
        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
        return context;
    }
}