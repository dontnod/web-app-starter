namespace WebAppStarter.Application.Common.Behaviours;

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using WebAppStarter.Application.Common.Interfaces;

public class PerformanceBehaviour<TRequest, TResponse>(
        ILogger<TRequest> logger,
        ICurrentUser user) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch timer = new Stopwatch();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        timer.Start();

        var response = await next();

        timer.Stop();

        var elapsedMilliseconds = timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = user.GetId().ToString() ?? string.Empty;
            var userName = user.GetDisplayName() ?? string.Empty;

            logger.LogWarning(
                "WebAppStarter Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName,
                elapsedMilliseconds,
                userId,
                userName,
                request);
        }

        return response;
    }
}
