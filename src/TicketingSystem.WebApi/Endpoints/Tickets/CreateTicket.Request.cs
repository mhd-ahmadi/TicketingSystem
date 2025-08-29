namespace TicketingSystem.WebApi.Endpoints.Tickets;

public class CreateTicketRequest
{
    public const string Route = "tickets";

    public string? Title { get; set; }
    public string? Description { get; set; }
    public short? PriorityId { get; set; }
}
