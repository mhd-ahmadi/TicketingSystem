using TicketingSystem.Domain.Common;

namespace TicketingSystem.Domain.Events;

public record TicketCreatedEvent(Guid TicketId) : IDomainEvent;
