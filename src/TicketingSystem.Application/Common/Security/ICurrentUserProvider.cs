using TicketingSystem.Application.Models;

namespace TicketingSystem.Application.Common.Security;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}
