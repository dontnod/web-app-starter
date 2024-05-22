namespace WebAppStarter.Application.Common.Behaviours;

using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using WebAppStarter.Application.Common.Interfaces;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, ICurrentUser user)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    Task IRequestPreProcessor<TRequest>.Process(
        TRequest request,
        CancellationToken cancellationToken
    )
    {
        var requestName = typeof(TRequest).Name;
        var userId = user.GetId().ToString() ?? string.Empty;
        string userName = user.GetDisplayName() ?? string.Empty;

        logger.LogInformation(
            "WebAppStarter Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName,
            userId,
            userName,
            request
        );

        return Task.CompletedTask;
    }
}
