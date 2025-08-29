namespace TicketingSystem.Application.Common;

public record PaginatedResult<T>(int TotalCount)
{
    public List<T> Items { get; set; } = [];
}