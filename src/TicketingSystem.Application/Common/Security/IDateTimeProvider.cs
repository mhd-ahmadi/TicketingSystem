namespace TicketingSystem.Application.Common.Security;

public interface IDateTimeProvider
{
    public DateTime Now { get; }
}