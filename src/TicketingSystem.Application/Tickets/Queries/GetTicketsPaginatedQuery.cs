using Mapster;
using MediatR;
using TicketingSystem.Application.Common;
using TicketingSystem.Application.Common.Security;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Application.Tickets.Dto;

namespace TicketingSystem.Application.Tickets.Queries;

public record GetTicketsPaginatedQuery(bool CreatedByCurrentUser, int Offset = 0, int Limit = 50) : IRequest<PaginatedResult<GetTicketQueryResultDto>>;

internal class GetTicketsPaginatedQueryHandler(
    ITicketRepository ticketRepository,
    ICurrentUserProvider currentUserProvider
    ) : IRequestHandler<GetTicketsPaginatedQuery, PaginatedResult<GetTicketQueryResultDto>>
{
    public async Task<PaginatedResult<GetTicketQueryResultDto>> Handle(GetTicketsPaginatedQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = request.CreatedByCurrentUser
            ? currentUserProvider.GetCurrentUser().Id
            : null;

        int totalCount = userId is not null
            ? await ticketRepository.GetTotalCountAsync(userId.Value, cancellationToken)
            : await ticketRepository.GetTotalCountAsync(cancellationToken);

        var result = new PaginatedResult<GetTicketQueryResultDto>(totalCount);
        if (totalCount == 0)
            return result;

        var tickets = userId is not null
            ? await ticketRepository.GetByUserIdAsync(userId.Value, request.Offset, request.Limit, cancellationToken)
            : await ticketRepository.GetListAsync(request.Offset, request.Limit, cancellationToken);

        result.Items = tickets.Adapt<List<GetTicketQueryResultDto>>();

        return result;
    }
}
