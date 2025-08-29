namespace TicketingSystem.Application.Models;

public record CurrentUser(
    Guid Id, 
    string FullName, 
    string Email, 
    IReadOnlyList<string> Roles
    );
