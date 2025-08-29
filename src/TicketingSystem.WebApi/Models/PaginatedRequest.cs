namespace TicketingSystem.WebApi.Models;

public class PaginatedRequest
{
    public int Offset { get; set; } = 0;

    public int Limit { get; set; } = 20;
}
