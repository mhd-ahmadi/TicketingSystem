using TicketingSystem.Domain.Common;

namespace TicketingSystem.Domain.Exceptions.Users;

public class InvalidPasswordException : DomainException
{
    public InvalidPasswordException() : base("Password is incorrect.") { }
}