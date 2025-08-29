using TicketingSystem.Domain.Common;

namespace TicketingSystem.Domain.Exceptions;

public class EntityNotFoundException(string entityName, Guid entityId) : 
    DomainException($"{entityName} with id '{entityId}' not found.");
