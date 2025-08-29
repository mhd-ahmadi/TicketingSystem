using TicketingSystem.Domain.Common;

namespace TicketingSystem.Domain.Exceptions.Users;

public class UserNotFoundException(string email) : DomainException($"User with email '{email}' not found.")
{
}
