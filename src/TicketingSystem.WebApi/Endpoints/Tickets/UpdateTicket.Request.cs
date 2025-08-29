using Microsoft.AspNetCore.Mvc;

namespace TicketingSystem.WebApi.Endpoints.Tickets;

public class UpdateTicketRequest
{
    public const string Route = "/tickets/{id:guid}";

    [FromRoute]
    public Guid Id { get; set; }
    public Guid? AssignToUserId { get; set; }
    public short? NewStatusId { get; set; }
}
