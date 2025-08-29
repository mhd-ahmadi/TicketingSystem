using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TicketingSystem.Domain.Common;
using TicketingSystem.Infrastructure.Middlewares;

namespace TicketingSystem.Infrastructure.Persistence.Interceptors;

public class DomainEventsInterceptor(IHttpContextAccessor httpContextAccessor, IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var domainEvents = eventData.Context.ChangeTracker
            .Entries<Entity>()
            .SelectMany(e => e.Entity.PopDomainEvents())
            .ToList();

        if (domainEvents.Count == 0)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            foreach (var domainEvent in domainEvents)
                await publisher.Publish(domainEvent, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private bool IsUserWaitingOnline() => httpContextAccessor.HttpContext is not null;

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        var queue = httpContextAccessor.HttpContext!.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value)
            && value is Queue<IDomainEvent> existing
                ? existing
                : new Queue<IDomainEvent>();

        foreach (var e in domainEvents)
            queue.Enqueue(e);

        httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = queue;
    }
}