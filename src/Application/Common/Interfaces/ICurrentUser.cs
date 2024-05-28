namespace WebAppStarter.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid? GetId();

    string? GetDisplayName();

    // What's that? -> src/Api/Services/CurrentUser.cs commented here, should be here too
    bool IsApplication();
}
