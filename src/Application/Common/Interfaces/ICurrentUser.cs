namespace WebAppStarter.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid? GetId();

    string? GetDisplayName();

    bool IsApplication();
}
