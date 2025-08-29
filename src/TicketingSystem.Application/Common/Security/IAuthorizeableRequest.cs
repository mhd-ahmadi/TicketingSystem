using MediatR;

namespace TicketingSystem.Application.Common.Security;

public interface IAuthorizeableRequest<T> : IRequest<T>
{
    Guid UserId { get; }
}