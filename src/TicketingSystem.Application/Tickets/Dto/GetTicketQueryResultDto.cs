using TicketingSystem.Domain.Enums;

namespace TicketingSystem.Application.Tickets.Dto;

public class GetTicketQueryResultDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public TicketStatus? Status { get; set; }
    public TicketPriority? Priority { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
