using FastEndpoints;
using MediatR;
using TicketingSystem.Application.Tickets.Commands;
using TicketingSystem.Domain.Constants;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.WebApi.Endpoints.Tickets
{
    public class CreateTicket(ISender sender) : Endpoint<CreateTicketRequest, CreateTicketResponse>
    {
        public override void Configure()
        {
            Post(CreateTicketRequest.Route);
            Roles(RoleNames.Employee);
            Description(x => x.WithTags("Ticket"));
        }

        public override async Task HandleAsync(CreateTicketRequest req, CancellationToken ct)
        {
            var ticketId = await sender.Send(
                new CreateTicketCommand(req.Title!, req.Description!, (TicketPriority)req.PriorityId!.Value),
                ct);

            await Send.OkAsync(new CreateTicketResponse(ticketId), ct);
        }
    }
}
