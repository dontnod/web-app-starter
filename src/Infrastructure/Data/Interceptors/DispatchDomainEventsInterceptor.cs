namespace WebAppStarter.Infrastructure.Data.Interceptors;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebAppStarter.Domain.Common;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        await DispatchDomainEvents(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null)
            return;

        var entities = context
            .ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity);

        // too much linq is too much. I´d prefer we NEVER use Linq.ForEach
        foreach (var e in entities)
        {
            foreach (var domainEvent in e.DomainEvents)
            {
                await mediator.Publish(domainEvent);
            }

            e.ClearDomainEvents();
        }
    }
}
