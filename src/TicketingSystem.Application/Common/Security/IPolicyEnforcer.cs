using ErrorOr;
using TicketingSystem.Application.Models;

namespace TicketingSystem.Application.Common.Security;

public interface IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy);
}