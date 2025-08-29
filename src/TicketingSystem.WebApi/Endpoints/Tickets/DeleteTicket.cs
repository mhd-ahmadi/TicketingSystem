using FastEndpoints;
using MediatR;
using TicketingSystem.Application.Tickets.Commands;
using TicketingSystem.Domain.Constants;

namespace TicketingSystem.WebApi.Endpoints.Tickets;

public class DeleteTicket(ISender sender) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Roles(RoleNames.Admin);
        Verbs(Http.DELETE);
        Delete("/tickets/{id:guid}");
        Description(x => x.WithTags("Ticket"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await sender.Send(new DeleteTicketCommand(Route<Guid>("id")), ct);

        await Send.NoContentAsync(ct);
    }
}
