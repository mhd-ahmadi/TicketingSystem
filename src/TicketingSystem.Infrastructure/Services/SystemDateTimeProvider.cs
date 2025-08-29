using TicketingSystem.Application.Common.Security;

namespace TicketingSystem.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
