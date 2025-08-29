using FastEndpoints;
using MediatR;
using TicketingSystem.Application.Common;
using TicketingSystem.Application.Tickets.Dto;
using TicketingSystem.Application.Tickets.Queries;
using TicketingSystem.Domain.Constants;
using TicketingSystem.WebApi.Models;

namespace TicketingSystem.WebApi.Endpoints.Tickets;

public class MyTickets(ISender sender) : Endpoint<PaginatedRequest, PaginatedResult<GetTicketQueryResultDto>>
{
    public override void Configure()
    {
        Get("tickets/my");
        Description(x => x.WithTags("Ticket"));
        Roles(RoleNames.Employee);
    }

    public override async Task HandleAsync(PaginatedRequest req, CancellationToken ct)
    {
        var result = await sender.Send(
            new GetTicketsPaginatedQuery(CreatedByCurrentUser: true, req.Offset, req.Limit), ct);

        await Send.OkAsync(result, ct);
    }
}
