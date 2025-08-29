using FastEndpoints;
using TicketingSystem.Application.Repositories;
using TicketingSystem.Domain.Constants;
using TicketingSystem.Domain.Enums;

namespace TicketingSystem.WebApi.Endpoints.Tickets;

public class TicketsStatistics(ITicketRepository ticketRepository) : EndpointWithoutRequest<Dictionary<string, int>>
{
    public override void Configure()
    {
        Roles(RoleNames.Admin);
        Get("tickets/stats");
        Description(x => x.WithTags("Ticket"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var counts = await ticketRepository.GetTicketCountsByStatusAsync(ct);

        Response = TicketStatus.List.ToDictionary(
            status => status.Name,
            status => counts.TryGetValue(status, out var count) ? count : 0
        );
    }
}
