using FastEndpoints;
using MediatR;
using TicketingSystem.Application.Tickets.Commands;
using TicketingSystem.Domain.Constants;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.WebApi.Endpoints.Tickets
{
    public class UpdateTicket(ISender sender) : Endpoint<UpdateTicketRequest>
    {
        public override void Configure()
        {
            Roles(RoleNames.Admin);
            Put(UpdateTicketRequest.Route);
            Description(x => x.WithTags("Ticket"));
        }

        public override async Task HandleAsync(UpdateTicketRequest req, CancellationToken ct)
        {
            await sender.Send(new UpdateTicketCommand(req.Id, req.AssignToUserId, 
                req.NewStatusId.HasValue ? TicketStatus.FromValue(req.NewStatusId!.Value) : null),
                ct);

            await Send.NoContentAsync(ct);
        }
    }
}
