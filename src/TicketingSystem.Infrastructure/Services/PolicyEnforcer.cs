using ErrorOr;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Application.Models;
using TicketingSystem.Domain.Constants;

namespace TicketingSystem.Infrastructure.Services;

public class PolicyEnforcer : IPolicyEnforcer
{
    public ErrorOr<Success> Authorize<T>(
        IAuthorizeableRequest<T> request,
        CurrentUser currentUser,
        string policy)
    {
        return policy switch
        {
            Policy.SelfOrAdmin => SelfOrAdminPolicy(request, currentUser),
            _ => Error.Unexpected(description: "Unknown policy name"),
        };
    }

    private static ErrorOr<Success> SelfOrAdminPolicy<T>(IAuthorizeableRequest<T> request, CurrentUser currentUser) =>
        request.UserId == currentUser.Id || currentUser.Roles.Contains(RoleNames.Admin)
            ? Result.Success
            : Error.Unauthorized(description: "Requesting user failed policy requirement");
}