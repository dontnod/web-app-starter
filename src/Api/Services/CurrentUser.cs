namespace WebAppStarter.Api.Services;

using System.Security.Claims;
using Microsoft.Identity.Web;
using WebAppStarter.Application.Common.Interfaces;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetId()
    {
        Guid? userId = null;
        var user = httpContextAccessor.HttpContext?.User;

        if (user != null)
        {
            userId = Guid.Parse(user.GetObjectId()!); // How sure are we this can't be null? Seems not costly to check?
        }

        return userId;
    }

    public string? GetDisplayName()
    {
        string? displayName = null;
        var user = httpContextAccessor.HttpContext?.User;

        if (user != null)
        {
            displayName = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        return displayName;
    }

    /// <summary>
    /// Access tokens that have neither the 'scp' (for delegated permissions) nor
    /// 'roles' (for application permissions) claim are not to be honored.
    ///
    /// An access token issued by Azure AD will have at least one of the two claims. Access tokens
    /// issued to a user will have the 'scp' claim. Access tokens issued to an application will have
    /// the roles claim. Access tokens that contain both claims are issued only to users, where the scp
    /// claim designates the delegated permissions, while the roles claim designates the user's role.
    ///
    /// To determine whether an access token was issued to a user (i.e delegated) or an application
    /// more easily, we recommend enabling the optional claim 'idtyp'. For more information, see:
    /// https://docs.microsoft.com/azure/active-directory/develop/access-tokens#user-and-application-tokens.
    /// </summary>
    /// <returns>True if the current user is an application.</returns>
    public bool IsApplication()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null)
        {
            return false;
        }

        // Add in the optional 'idtyp' claim to check if the access token is coming from an application or user.
        // See: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-optional-claims
        if (user.Claims.Any(c => c.Type == "idtyp"))
        {
            return user.Claims.Any(c => c.Type == "idtyp" && c.Value == "app");
        }
        else
        {
            // alternatively, if an AT contains the roles claim but no scp claim, that indicates it's an app token
            return user.Claims.Any(c => c.Type == "roles")
                && !user.Claims.Any(c => c.Type == "scp");
        }
    }
}
